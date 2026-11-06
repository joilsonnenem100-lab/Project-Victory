using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
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
    private readonly FinalizeHuntAfterCombatHandler _finalizeHuntAfterCombatHandler;
    private readonly PlayerOwnershipGuard _ownershipGuard;
    private readonly ICharacterRepository _characterRepository;
    private readonly CombatantFactory _combatantFactory;
    private readonly CombatOrchestrator _orchestrator;
    private readonly RewardResolutionHandler _rewardResolutionHandler;


    public AttackEncounterHandler(
        IEncounterStateStore encounterStateStore,
        IHuntRepository huntRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ICombatResultStore combatResultStore,
        FinalizeHuntAfterCombatHandler finalizeHuntAfterCombatHandler,
        PlayerOwnershipGuard ownershipGuard,
        ICharacterRepository characterRepository,
        CombatantFactory combatantFactory,
        CombatOrchestrator orchestrator,
        RewardResolutionHandler rewardResolutionHandler)
    {
        _ownershipGuard = ownershipGuard;
        _encounterStateStore = encounterStateStore;
        _huntRepository = huntRepository;
        _runtimeExecutionStateStore = runtimeExecutionStateStore;
        _combatResultStore = combatResultStore;
        _finalizeHuntAfterCombatHandler = finalizeHuntAfterCombatHandler;
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

        switch (report.Outcome)
        {
            case CombatSessionOutcome.Victory:
                encounter.Victory();
                hunt.CompleteAfterCombat();
                break;

            case CombatSessionOutcome.Defeat:
                encounter.Defeat();
                break;
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
            report.Log);
    }
}
