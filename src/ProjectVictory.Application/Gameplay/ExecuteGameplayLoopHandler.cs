using ProjectVictory.Domain.Gameplay;

namespace ProjectVictory.Application.Gameplay;

public sealed class ExecuteGameplayLoopHandler
{
    public ExecuteGameplayLoopResult Handle(
        ExecuteGameplayLoopCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.HuntId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "HuntId is required.");
        }

        var session = new GameplaySessionContext(
            Guid.NewGuid(),
            command.CharacterId);

        var selection = new SelectHuntHandler()
            .Handle(
                new SelectHuntCommand(
                    session.SessionId,
                    command.HuntId));

        return new ExecuteGameplayLoopResult(
            command.CharacterId,
            selection.HuntId,
            selection.Selected);
    }
}