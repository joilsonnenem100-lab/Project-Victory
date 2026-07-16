namespace ProjectVictory.Application.VerticalSlice;

public sealed class ConsolidateVerticalSliceHandler
{
    public VerticalSliceFinalState Handle(
        ConsolidateVerticalSliceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new VerticalSliceFinalState(
            command.CharacterId,
            command.FlowCompleted);
    }
}