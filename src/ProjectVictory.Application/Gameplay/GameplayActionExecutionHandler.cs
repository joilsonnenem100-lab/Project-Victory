using ProjectVictory.Application.Session;
using ProjectVictory.Application.World;

namespace ProjectVictory.Application.Gameplay;

public sealed class GameplayActionExecutionHandler
{
    private readonly GameplayActionDispatcher _dispatcher;
    private readonly PlayerOwnershipGuard _ownershipGuard;

    public GameplayActionExecutionHandler(
        GameplayActionDispatcher dispatcher,
        PlayerOwnershipGuard ownershipGuard)
    {
        _dispatcher = dispatcher;
        _ownershipGuard = ownershipGuard;
    }

    public async Task<GameplayActionExecutionResult> Handle(
        GameplayActionExecutionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        // PV-SEC-001: o personagem precisa pertencer à conta autenticada.
        await _ownershipGuard.EnsureCharacterAsync(
            command.CharacterId);

        var result = await _dispatcher.Handle(
            new ExecuteLocationActionCommand(
                command.ActionId,
                command.CharacterId));

        return new GameplayActionExecutionResult(
            result.ActionId,
            result.System);
    }
}
