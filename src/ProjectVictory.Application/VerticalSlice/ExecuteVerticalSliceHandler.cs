namespace ProjectVictory.Application.VerticalSlice;

public sealed class ExecuteVerticalSliceHandler
{
    public ExecuteVerticalSliceResult Handle(
        ExecuteVerticalSliceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ExecuteVerticalSliceResult(
            command.CharacterId,
            true);
    }
}