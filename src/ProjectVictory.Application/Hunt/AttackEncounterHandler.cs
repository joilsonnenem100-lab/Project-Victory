using ProjectVictory.Application.Checkpoint;
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
    private readonly IPendingSupplyConsumption _pendingSupplies;
    private readonly ICheckpointService _checkpoints;
    private readonly ProjectVictory.Application.GameSession.IGameActivity _activity;
    private readonly IHuntLock _huntLock;
    private readonly EncounterGenerator _encounterGenerator;
    private readonly ICombatSeedProvider _seedProvider;
    private readonly IActiveSpawnRegistry _activeSpawns;
    private readonly ProjectVictory.Application.Spawn.ISpawnSystem _spawnSystem;
    private readonly TimeProvider _time;


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
        CombatSupplyContext supplies,
        // PV-PERSIST-011: consumo pendente e checkpoints (Handbook §19).
        IPendingSupplyConsumption pendingSupplies,
        ICheckpointService checkpoints,
        // PV-RUNTIME-010: cada ataque também conta como presença.
        ProjectVictory.Application.GameSession.IGameActivity activity,
        // PV-HUNT-011: Hunt Loop contínuo (próximo encounter pelo Spawn) e serialização por Hunt.
        IHuntLock huntLock,
        EncounterGenerator encounterGenerator,
        ICombatSeedProvider seedProvider,
        IActiveSpawnRegistry activeSpawns,
        ProjectVictory.Application.Spawn.ISpawnSystem spawnSystem,
        TimeProvider time)
    {
        _time = time;
        _activity = activity;
        _huntLock = huntLock;
        _encounterGenerator = encounterGenerator;
        _seedProvider = seedProvider;
        _activeSpawns = activeSpawns;
        _spawnSystem = spawnSystem;
        _pendingSupplies = pendingSupplies;
        _checkpoints = checkpoints;
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

        // PV-HUNT-011 (PV-STATE-001 D3): um comando por vez sobre a mesma Hunt (ataque, saída, queda).
        var huntId = _encounterStateStore.Get(command.EncounterId).HuntId;

        using (await _huntLock.AcquireAsync(huntId, cancellationToken))
        {
            // Enquanto esperava, a Hunt pode ter acabado (encounter descartado).
            if (!_encounterStateStore.Contains(command.EncounterId))
            {
                throw new InvalidOperationException("Combat already finished.");
            }

            return await HandleLocked(command, cancellationToken);
        }
    }

    private async Task<AttackEncounterResult> HandleLocked(
        AttackEncounterCommand command,
        CancellationToken cancellationToken)
    {

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

        _activity.MarkActive(character.Id);

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

        // PV-PERSIST-011: poções já usadas e ainda não gravadas (pendentes até o próximo checkpoint)
        // não estão mais disponíveis.
        var unsaved = _pendingSupplies.Peek(character.Id);

        _supplies.Load(
            policy,
            Math.Max(0, potions.Count(item => item.ItemDefinitionId == SupplyCatalog.HealthPotionId) - unsaved.HealthPotions),
            Math.Max(0, potions.Count(item => item.ItemDefinitionId == SupplyCatalog.ManaPotionId) - unsaved.ManaPotions));
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

        // PV-SUPPLY-010 + PV-PERSIST-011 (Handbook §19): poções usadas na rodada saem do inventário,
        // mas a rodada é "ação pequena" — o consumo fica PENDENTE e é gravado no próximo checkpoint
        // (recompensa ou conclusão). Antes: gravava o personagem a cada rodada com poção.
        _pendingSupplies.Add(character.Id, _supplies.HealthPotionsUsed, _supplies.ManaPotionsUsed);

        // Monstros mortos nesta rodada → Reward Pipeline (um CombatResult de vitória por monstro).
        var killed = session.Monsters.Where(m => m.IsDead && aliveBefore.Contains(m.Id)).ToList();

        // PV-PERSIST-011: checkpoint de RECOMPENSA antes do Reward Pipeline (que recarrega o personagem).
        if (killed.Count > 0)
        {
            await _checkpoints.SaveAsync(character.Id, hunt.RuntimeId, CheckpointReason.Reward, cancellationToken);
        }
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

        // PV-HUNT-011: XP desta rodada entra na conta da Hunt (XP/hora).
        hunt.RecordExperience(experienceGained);

        // PV-HUNT-010/011: a morte encerra a Hunt pelo pipeline único (ADR-012). A VITÓRIA de um
        // encounter NÃO encerra mais a Hunt (DEC-20261106-22, fecha o R-036): o encounter vencido é
        // despawnado e descartado, e o Spawn gera o PRÓXIMO (Hunt Loop contínuo).
        EndHuntResult? ending = null;
        ActiveEncounter? next = null;

        switch (report.Outcome)
        {
            case CombatSessionOutcome.Victory:
                encounter.Victory();
                _spawnSystem.Despawn(_activeSpawns.Release(encounter.Id));
                _encounterStateStore.Remove(encounter.Id);
                next = _encounterGenerator.Generate(hunt.Id, _seedProvider.NextSeed());
                _encounterStateStore.Add(next);
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
            ending?.DeathNotice,
            next?.Id,
            next is null ? null : ToResult(next),
            (long)Math.Floor(hunt.ElapsedAt(_time.GetUtcNow()).TotalSeconds),
            hunt.ExperienceGained);
    }

    private static EncounterResult ToResult(ActiveEncounter encounter)
    {
        var target = encounter.GetCurrentTarget();

        return new EncounterResult(
            encounter.Id,
            encounter.HuntId,
            encounter.CurrentTargetId,
            target.Name,
            target.CurrentHealth,
            target.MaximumHealth);
    }
}
