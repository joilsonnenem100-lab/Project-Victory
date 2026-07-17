namespace ProjectVictory.Application.VerticalSlice;

public sealed class EnhancePlayerFlowHandler
{
    private readonly ConfirmPlayableVerticalSliceHandler _handler;

    public EnhancePlayerFlowHandler()
    {
        _handler = new ConfirmPlayableVerticalSliceHandler();
    }

    public EnhancePlayerFlowResult Handle(
        EnhancePlayerFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = _handler.Handle(
            new ConfirmPlayableVerticalSliceCommand(
                command.CharacterId));

        return new EnhancePlayerFlowResult(
            result.CharacterId,
            result.Confirmed);
    }
}