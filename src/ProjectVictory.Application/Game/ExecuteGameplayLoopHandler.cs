using ProjectVictory.Application.Hunt;

namespace ProjectVictory.Application.Game;

public sealed class ExecuteGameplayLoopHandler
{
    private readonly EnterGameHandler _enterGameHandler;
    private readonly ExecuteHuntHandler _executeHuntHandler;

    public ExecuteGameplayLoopHandler(
        EnterGameHandler enterGameHandler,
        ExecuteHuntHandler executeHuntHandler)
    {
        _enterGameHandler = enterGameHandler;
        _executeHuntHandler = executeHuntHandler;
    }

    public async Task<ExecuteGameplayLoopResult> Handle(
        ExecuteGameplayLoopCommand command,
        CancellationToken cancellationToken = default)
    {
        var entryResult = await _enterGameHandler.Handle(
            new EnterGameCommand(command.AccountId),
            cancellationToken);

        var huntResult = await _executeHuntHandler.Handle(
            new ExecuteHuntCommand(
                entryResult.CharacterId,
                command.EnemyId),
            cancellationToken);

        if (!huntResult.Completed)
        {
            throw new InvalidOperationException(
                "Gameplay loop hunt failed.");
        }

        return new ExecuteGameplayLoopResult(
            entryResult.AccountId,
            entryResult.CharacterId,
            huntResult.HuntId,
            huntResult.Experience,
            huntResult.Level);
    }
}