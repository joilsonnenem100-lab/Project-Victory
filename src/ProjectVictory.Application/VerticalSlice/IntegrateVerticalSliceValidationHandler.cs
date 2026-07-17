namespace ProjectVictory.Application.VerticalSlice;

public sealed class IntegrateVerticalSliceValidationHandler
{
    private readonly ExecuteVerticalSliceHandler _executeHandler;
    private readonly ValidateVerticalSliceBoundariesHandler _boundaryHandler;
    private readonly CreateGameplayValidationResultHandler _resultHandler;

    public IntegrateVerticalSliceValidationHandler()
    {
        _executeHandler = new ExecuteVerticalSliceHandler();
        _boundaryHandler = new ValidateVerticalSliceBoundariesHandler();
        _resultHandler = new CreateGameplayValidationResultHandler();
    }

    public IntegrateVerticalSliceValidationResult Handle(
        IntegrateVerticalSliceValidationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var execution = _executeHandler.Handle(
            new ExecuteVerticalSliceCommand(
                command.CharacterId));

        var boundaries = _boundaryHandler.Handle();

        var validation = _resultHandler.Handle(
            new CreateGameplayValidationResultCommand(
                command.CharacterId,
                execution.Completed,
                boundaries.HuntValid,
                boundaries.CharacterValid,
                boundaries.RewardValid,
                boundaries.ProgressionValid));

        return new IntegrateVerticalSliceValidationResult(
            command.CharacterId,
            validation.Completed,
            validation);
    }
}