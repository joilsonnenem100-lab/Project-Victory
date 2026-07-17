namespace ProjectVictory.Application.VerticalSlice;

public sealed class ConfirmPlayableVerticalSliceHandler
{
    private readonly ExecuteFirstGameplayValidationHandler _handler;

    public ConfirmPlayableVerticalSliceHandler()
    {
        _handler =
            new ExecuteFirstGameplayValidationHandler();
    }

    public ConfirmPlayableVerticalSliceResult Handle(
        ConfirmPlayableVerticalSliceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validation =
            _handler.Handle(
                new ExecuteFirstGameplayValidationCommand(
                    command.CharacterId));

        return new ConfirmPlayableVerticalSliceResult(
            validation.CharacterId,
            validation.Validated);
    }
}