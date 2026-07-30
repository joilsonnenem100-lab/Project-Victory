using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Hunt;

namespace ProjectVictory.Application.Encounter;

public sealed class StartEncounterCombatHandler
{
    private readonly IEncounterStateStore _encounterStateStore;
    private readonly CreateCombatContextHandler _createCombatContextHandler;
    private readonly BindEncounterToCombatHandler _bindEncounterToCombatHandler;

    public StartEncounterCombatHandler(
        IEncounterStateStore encounterStateStore,
        CreateCombatContextHandler createCombatContextHandler,
        BindEncounterToCombatHandler bindEncounterToCombatHandler)
    {
        _encounterStateStore = encounterStateStore;
        _createCombatContextHandler = createCombatContextHandler;
        _bindEncounterToCombatHandler = bindEncounterToCombatHandler;
    }

    public async Task<StartEncounterCombatResult> Handle(
        StartEncounterCombatCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var encounter =
            _encounterStateStore.Get(
                command.EncounterId);

        var target =
    encounter.GetCurrentTarget();

        var combat =
            await _createCombatContextHandler.Handle(
                new CreateCombatContextCommand(
                    encounter.HuntId,
                    command.CharacterId,
                    target.MonsterId),
                cancellationToken);

        _bindEncounterToCombatHandler.Handle(
            new BindEncounterToCombatCommand(
                encounter.Id,
                combat.CombatContextId));

        return new StartEncounterCombatResult(
            encounter.Id,
            combat.CombatContextId);
    }
}
