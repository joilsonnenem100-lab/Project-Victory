namespace ProjectVictory.Application.VerticalSlice;

public sealed class ExecuteFirstGameplayValidationHandler
{
    private readonly PrepareFirstPlayableScenarioHandler _prepareHandler;
    private readonly ExecuteVerticalSliceHandler _executeHandler;

    public ExecuteFirstGameplayValidationHandler()
    {
        _prepareHandler =
            new PrepareFirstPlayableScenarioHandler();

        _executeHandler =
            new ExecuteVerticalSliceHandler();
    }

    public ExecuteFirstGameplayValidationResult Handle(
        ExecuteFirstGameplayValidationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var preparation =
            _prepareHandler.Handle(
                new PrepareFirstPlayableScenarioCommand(
                    command.CharacterId));

        if (!preparation.Prepared)
        {
            return new ExecuteFirstGameplayValidationResult(
                command.CharacterId,
                false);
        }

        var execution =
            _executeHandler.Handle(
                new ExecuteVerticalSliceCommand(
                    command.CharacterId));

        return new ExecuteFirstGameplayValidationResult(
            execution.CharacterId,
            execution.Completed);
    }
}