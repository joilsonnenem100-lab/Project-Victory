using System;

namespace ProjectVictory.Application.World;

public sealed class ExplorationDecisionHandler
{
    private readonly GameplayActionDispatcher _dispatcher;

    public ExplorationDecisionHandler(
        GameplayActionDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<ExplorationDecisionResult> Handle(
        ExplorationDecisionCommand command)
    {
        if (command.RuntimeId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Runtime context not found.");
        }

        if (command.ActionId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Exploration action not found.");
        }

        var result = await _dispatcher.Handle(
            new ExecuteLocationActionCommand(
                command.ActionId,
                command.CharacterId));

        return new ExplorationDecisionResult(
            command.RuntimeId,
            result.ActionId,
            command.CharacterId,
            result.System);
    }
}
