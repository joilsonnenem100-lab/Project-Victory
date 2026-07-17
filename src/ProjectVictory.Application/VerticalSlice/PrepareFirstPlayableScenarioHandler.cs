namespace ProjectVictory.Application.VerticalSlice;

public sealed class PrepareFirstPlayableScenarioHandler
{
    public PrepareFirstPlayableScenarioResult Handle(
        PrepareFirstPlayableScenarioCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new PrepareFirstPlayableScenarioResult(
            command.CharacterId,
            true);
    }
}