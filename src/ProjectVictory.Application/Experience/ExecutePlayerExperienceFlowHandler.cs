namespace ProjectVictory.Application.Experience;

public sealed class ExecutePlayerExperienceFlowHandler
{
    public ExecutePlayerExperienceFlowResult Handle(
        ExecutePlayerExperienceFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var context = new CreatePlayerExperienceContextCommand(
            command.CharacterId,
            command.WorldId,
            null);

        var contextHandler =
            new CreatePlayerExperienceContextHandler();

        var result =
            contextHandler.Handle(context);

        return new ExecutePlayerExperienceFlowResult(
            result.CharacterId,
            result.Created);
    }
}