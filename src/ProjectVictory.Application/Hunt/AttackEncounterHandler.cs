using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.Supply;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Application.Hunt;

// PV-COMBAT-012: o comando "attack" executa uma rodada do combate por turnos real (Combat
// Orchestrator, ADR-051/052): o turno do jogador e o dos monstros, até a vez do jogador voltar.
// Antes: dano fixo 5 no alvo, monstros nunca atacavam e o jogador nunca perdia vida.
//
// Cada monstro morto gera um CombatResult de vitória entregue ao Reward Pipeline (ADR-024):
// XP, ouro e loot da definição daquele monstro. Vitória = todos os monstros do encounter mortos.
public sealed class AttackEncounterHandler
{
    private readonly IEncounterStateStore _encounterStateStore;
    private readonly IHuntRepository _huntRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly ICombatResultStore _combatResultStore;
    private readonly PlayerOwnershipGuard _ownershipGuard;
    private readonly ICharacterRepository _characterRepository;
    private readonly CombatantFactory _combatantFactory;
    private readonly CombatOrchestrator _orchestrator;
    private readonly RewardResolutionHandler _rewardResolutionHandler;
    private readonly EndHuntHandler _endHuntHandler;
    private readonly ISupplyPolicyStore _supplyPolicies;
    private readonly CombatSupplyContext _supplies;


    public AttackEncounterHandler(
        IEncounterStateStore encounterStateStore,
        IHuntRepository huntRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ICombatResultStore combatResultStore,
        PlayerOwnershipGuard ownershipGuard,
        ICharacterRepository characterRepository,
        CombatantFactory combatantFactory,
        CombatOrchestrator orchestrator,
        RewardResolutionHandler rewardResolutionHandler,
        EndHuntHandler endHuntHandler,
        ISupplyPolicyStore supplyPolicies,
        CombatSupplyContext supplies)
    {
        _supplyPolicies = supplyPolicies;
        _supplies = supplies;
        _endHuntHandler = endHuntHandler;
        _ownershipGuard = ownershipGuard;
        _encounterStateStore = encounterStateStore;
        _huntRepository = huntRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _combatResultStore = combatResultStore;
        _characterRepository = characterRepository;
        _combatantFactory = combatantFactory;
        _orchestrator = orchestrator;
        _rewardResolutionHandler = rewardResolutionHandler;
    }


    public async Task<AttackEncounterResult> Handle(
        AttackEncounterCommand command,
        CancellationToken cancellationToken = default)
    {
        // PV-SEC-001: o encounter precisa ser de uma hunt de personagem da conta autenticada.
        await _ownershipGuard.EnsureEncounterAsync(
            command.EncounterId,
            cancellationToken);

        var encounter =
            _encounterStateStore.Get(
                command.EncounterId);

        var hunt =
            await _huntRepository.GetByIdAsync(
                encounter.HuntId,
                cancellationToken)
            ?? throw new InvalidOperationException("Hunt not found.");

        var runtimeContext =
            _runtimeExecutionStateStore.Get(
                hunt.RuntimeId);

        if (runtimeContext.CharacterState is null)
        {
            throw new InvalidOperationException(
                "Character runtime state not initialized.");
        }

        if (encounter.IsFinished())
        {
            throw new InvalidOperationException(
                "Combat already finished.");
        }

        var character =
            await _characterRepository.GetByIdAsync(
                hunt.CharacterId,
                cancellationToken)
            ?? throw new InvalidOperationException("Character not found.");

        encounter.Activate();

        // PV-COMBAT-011/012: a sessão nasce no primeiro ataque, com a semente do encounter.
        if (encounter.Session is null)
        {
            encounter.AttachSession(
                new CombatSession(
                    Guid.NewGuid(),
                    _combatantFactory.CreatePlayer(character, runtimeContext.CharacterState),
                    encounter.Monsters.Select(_combatantFactory.CreateMonster).ToList(),
                    new SeededRandomSource(encounter.Seed)));
        }

        var session = encounter.Session!;

        // PV-SUPPLY-010: a política gravada pelo jogador passa a valer AGORA, no início do próximo
        // turno dele (nunca no meio de um turno). As poções vêm do inventário do personagem.
        var policy = _supplyPolicies.ActivatePending(character.Id);
        var potions = character.Inventory.Items
            .Where(item => SupplyCatalog.IsPotion(item.ItemDefinitionId))
            .ToList();

        _supplies.Load(
            policy,
            potions.Count(item => item.ItemDefinitionId == SupplyCatalog.HealthPotionId),
            potions.Count(item => item.ItemDefinitionId == SupplyCatalog.ManaPotionId));
        var aliveBefore = session.Monsters.Where(m => !m.IsDead).Select(m => m.Id).ToHashSet();

        var report = _orchestrator.ExecuteRound(session);

        // Estado do combate → estado do encounter e do runtime (vida/mana atuais do jogador).
        foreach (var monster in encounter.Monsters)
        {
            var combatant = session.Monsters.First(m => m.Id == monster.Id);
            var damage = monster.CurrentHealth - combatant.CurrentHealth;

            if (damage > 0)
            {
                monster.TakeDamage(damage);
            }
        }

        runtimeContext.CharacterState.UpdateHealth(session.Player.CurrentHealth);
        runtimeContext.CharacterState.UpdateMana(session.Player.CurrentMana);

        // PV-SUPPLY-010: poções usadas na rodada saem do inventário (antes das recompensas, que
        // recarregam o personagem).
        if (_supplies.HealthPotionsUsed + _supplies.ManaPotionsUsed > 0)
        {
            RemovePotions(character, SupplyCatalog.HealthPotionId, _supplies.HealthPotionsUsed);
            RemovePotions(character, SupplyCatalog.ManaPotionId, _supplies.ManaPotionsUsed);

            await _characterRepository.UpdateAsync(character, cancellationToken);
        }

        // Monstros mortos nesta rodada → Reward Pipeline (um CombatResult de vitória por monstro).
        var killed = session.Monsters.Where(m => m.IsDead && aliveBefore.Contains(m.Id)).ToList();
        long experienceGained = 0;

        foreach (var monster in killed)
        {
            var combatResult =
                new CombatResult(
                    Guid.NewGuid(),
                    hunt.Id,
                    encounter.Id,
                    hunt.CharacterId,
                    monster.SourceId,
                    CombatOutcome.Victory);

            _combatResultStore.Store(combatResult);

            var reward = await _rewardResolutionHandler.Handle(combatResult, cancellationToken);
            experienceGained += reward.ExperienceAmount ?? 0;
        }

        // PV-HUNT-010: vitória e morte encerram a Hunt pelo MESMO pipeline (ADR-012).
        EndHuntResult? ending = null;

        switch (report.Outcome)
        {
            case CombatSessionOutcome.Victory:
                encounter.Victory();
                ending = await _endHuntHandler.Handle(new EndHuntCommand(hunt.Id, HuntEndReason.Victory), cancellationToken);
                break;

            case CombatSessionOutcome.Defeat:
                encounter.Defeat();
                ending = await _endHuntHandler.Handle(new EndHuntCommand(hunt.Id, HuntEndReason.Death), cancellationToken);
                break;

            // PV-SUPPLY-011 (DEC-20261106-22): fim de suprimento NÃO encerra a Hunt (antes encerrava,
            // PV-SUPPLY-010). Sem poção o personagem segue sem cura até morrer.
        }

        var target =
            session.DefaultTargetFor(session.Player)
            ?? killed.LastOrDefault()
            ?? session.Monsters.Last();

        var characterAfter =
            await _characterRepository.GetByIdAsync(
                hunt.CharacterId,
                cancellationToken);

        return new AttackEncounterResult(
            encounter.Id,
            target.Name,
            target.CurrentHealth,
            target.MaximumHealth,
            killed.Count > 0,
            session.Player.CurrentHealth,
            // R-023: antes recebia a Mana atual por engano (argumentos posicionais).
            session.Player.MaximumHealth,
            session.Player.IsDead,
            encounter.IsFinished(),
            encounter.State.ToString(),
            checked((int)experienceGained),
            0,
            characterAfter?.Level.Value ?? character.Level.Value,
            [],
            report.Log.Select(CombatTurnDto.From).ToList(),
            ending?.Reason.ToString(),
            ending?.ReturnedToCity ?? false,
            ending?.ReturnCity,
            ending?.DeathNotice);
    }

    private static void RemovePotions(
        ProjectVictory.Domain.Entities.Character character,
        Guid potionDefinitionId,
        int amount)
    {
        var instances = character.Inventory.Items
            .Where(item => item.ItemDefinitionId == potionDefinitionId)
            .Take(amount)
            .Select(item => item.Id)
            .ToList();

        foreach (var instanceId in instances)
        {
            character.Inventory.RemoveItem(instanceId);
        }
    }
}
