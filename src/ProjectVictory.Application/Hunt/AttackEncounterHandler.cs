using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class AttackEncounterHandler
{
    private readonly IEncounterStateStore _encounterStateStore;
    private readonly IHuntRepository _huntRepository;
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;
    private readonly ICombatResultStore _combatResultStore;
    private readonly FinalizeHuntAfterCombatHandler _finalizeHuntAfterCombatHandler;

    private const int AttackDamage = 5;


    public AttackEncounterHandler(
        IEncounterStateStore encounterStateStore,
        IHuntRepository huntRepository,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ICombatResultStore combatResultStore,
        FinalizeHuntAfterCombatHandler finalizeHuntAfterCombatHandler)
    {
        _encounterStateStore = encounterStateStore;

        _huntRepository = huntRepository;

        _runtimeExecutionStateStore = runtimeExecutionStateStore;

        _combatResultStore = combatResultStore;

        _finalizeHuntAfterCombatHandler =
            finalizeHuntAfterCombatHandler;
    }


    public async Task<AttackEncounterResult> Handle(
        AttackEncounterCommand command,
        CancellationToken cancellationToken = default)
    {
        var encounter =
            _encounterStateStore.Get(
                command.EncounterId);


        var hunt =
            await _huntRepository.GetByIdAsync(
                encounter.HuntId,
                cancellationToken);


        if (hunt is null)
        {
            throw new InvalidOperationException(
                "Hunt not found.");
        }


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


        encounter.Activate();


        var target =
            encounter.GetCurrentTarget();


        target.TakeDamage(
            AttackDamage);


        var monsterDefeated =
            target.IsDead;


        if (monsterDefeated)
        {
            encounter.Victory();


            var combatResult =
                new CombatResult(
                    Guid.NewGuid(),
                    hunt.Id,
                    encounter.Id,
                    hunt.CharacterId,
                    target.MonsterId,
                    CombatOutcome.Victory);


            _combatResultStore.Store(
                combatResult);


            await _finalizeHuntAfterCombatHandler.Handle(
                new FinalizeHuntAfterCombatCommand(
                    hunt.Id),
                cancellationToken);
        }


        return new AttackEncounterResult(
            encounter.Id,
            target.Name,
            target.CurrentHealth,
            target.MaximumHealth,
            monsterDefeated,
            runtimeContext.CharacterState.CurrentHealth,
            runtimeContext.CharacterState.CurrentMana,
            false,
            encounter.IsFinished(),
            encounter.State.ToString(),
            0,
            0,
            0,
            []);
    }
}