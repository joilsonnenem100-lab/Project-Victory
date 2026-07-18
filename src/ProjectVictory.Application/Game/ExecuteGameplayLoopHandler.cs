using ProjectVictory.Application.Session;

namespace ProjectVictory.Application.Game;

public sealed class ExecuteGameplayLoopHandler
{
    private readonly EnterGameHandler _enterGameHandler;


    public ExecuteGameplayLoopHandler(
        EnterGameHandler enterGameHandler)
    {
        _enterGameHandler = enterGameHandler;
    }


    public async Task<EnterGameResult> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _enterGameHandler.Handle(
            new EnterGameCommand(),
            cancellationToken);
    }
}