namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidatePlayerExperienceHandler
{
    private readonly EnhancePlayerFlowHandler _enhancementHandler;
    private readonly CreateGameplayFeedbackHandler _feedbackHandler;

    public ValidatePlayerExperienceHandler()
    {
        _enhancementHandler =
            new EnhancePlayerFlowHandler();

        _feedbackHandler =
            new CreateGameplayFeedbackHandler();
    }

    public ValidatePlayerExperienceResult Handle(
        ValidatePlayerExperienceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var enhancement =
            _enhancementHandler.Handle(
                new EnhancePlayerFlowCommand(
                    command.CharacterId));

        var feedback =
            _feedbackHandler.Handle(
                new CreateGameplayFeedbackCommand(
                    command.CharacterId,
                    enhancement.Enhanced));

        return new ValidatePlayerExperienceResult(
            command.CharacterId,
            enhancement.Enhanced,
            feedback);
    }
}