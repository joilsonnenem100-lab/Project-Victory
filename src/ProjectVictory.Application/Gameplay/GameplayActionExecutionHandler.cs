using ProjectVictory.Application.World;

namespace ProjectVictory.Application.Gameplay;

public sealed class GameplayActionExecutionHandler
{
    private readonly GameplayActionDispatcher _dispatcher;

    public GameplayActionExecutionHandler(
        GameplayActionDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<GameplayActionExecutionResult> Handle(
        GameplayActionExecutionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = await _dispatcher.Handle(
            new ExecuteLocationActionCommand(
                command.ActionId,
                command.CharacterId));

        return new GameplayActionExecutionResult(
            result.ActionId,
            result.System);
    }
}
