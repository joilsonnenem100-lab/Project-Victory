namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateApplicationIntegrationHandler
{
    private readonly ExecuteVerticalSliceHandler _handler;

    public ValidateApplicationIntegrationHandler()
    {
        _handler = new ExecuteVerticalSliceHandler();
    }

    public ValidateApplicationIntegrationResult Handle(
        ValidateApplicationIntegrationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = _handler.Handle(
            new ExecuteVerticalSliceCommand(
                command.CharacterId));

        return new ValidateApplicationIntegrationResult(
            result.CharacterId,
            result.Completed);
    }
}