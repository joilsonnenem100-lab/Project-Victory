namespace ProjectVictory.Application.Gameplay;

public sealed class SelectHuntHandler
{
    public SelectHuntResult Handle(
        SelectHuntCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.SessionId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "SessionId is required.");
        }

        if (command.HuntId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "HuntId is required.");
        }

        return new SelectHuntResult(
            command.HuntId,
            true);
    }
}