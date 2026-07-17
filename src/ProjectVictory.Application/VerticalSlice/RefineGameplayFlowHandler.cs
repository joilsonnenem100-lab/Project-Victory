namespace ProjectVictory.Application.VerticalSlice;

public sealed class RefineGameplayFlowHandler
{
    private readonly ExecuteVerticalSliceHandler _handler;

    public RefineGameplayFlowHandler()
    {
        _handler = new ExecuteVerticalSliceHandler();
    }

    public RefineGameplayFlowResult Handle(
        RefineGameplayFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = _handler.Handle(
            new ExecuteVerticalSliceCommand(
                command.CharacterId));

        return new RefineGameplayFlowResult(
            result.CharacterId,
            result.Completed);
    }
}