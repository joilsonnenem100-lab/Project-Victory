using System;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Encounter;
using ProjectVictory.Application.Combat;
using ProjectVictory.Application.VerticalSlice;

namespace ProjectVictory.Application.World;

public sealed class GameplayActionDispatcher
{
    private readonly ExecuteHuntHandler _huntHandler;
    private readonly IRuntimeExecutionStateStore _runtimeStore;
    private readonly StartEncounterCombatHandler _startEncounterCombatHandler;
    private readonly ResolveCombatHandler _resolveCombatHandler;

    public GameplayActionDispatcher(
        ExecuteHuntHandler huntHandler,
        IRuntimeExecutionStateStore runtimeStore,
        StartEncounterCombatHandler startEncounterCombatHandler,
        ResolveCombatHandler resolveCombatHandler)
    {
        _huntHandler = huntHandler;
        _runtimeStore = runtimeStore;
        _startEncounterCombatHandler = startEncounterCombatHandler;
        _resolveCombatHandler = resolveCombatHandler;
    }

    public async Task<ExecuteLocationActionResult> Handle(
        ExecuteLocationActionCommand command)
    {
        if (command.ActionId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Location action not found.");
        }

        var runtime =
            _runtimeStore.GetByCharacterId(
                command.CharacterId);

        var huntResult =
            await _huntHandler.Handle(
                new ExecuteHuntCommand(
                    command.CharacterId,
                    runtime.RuntimeId));

        var combat =
            await _startEncounterCombatHandler.Handle(
                new StartEncounterCombatCommand(
                    huntResult.Encounter.Id,
                    command.CharacterId));

        await _resolveCombatHandler.Handle(
            new ResolveCombatCommand(
                combat.CombatContextId,
                command.CharacterId,
                huntResult.Encounter.CurrentTargetId));

        return new ExecuteLocationActionResult(
            command.ActionId,
            "Hunt");
    }
}
