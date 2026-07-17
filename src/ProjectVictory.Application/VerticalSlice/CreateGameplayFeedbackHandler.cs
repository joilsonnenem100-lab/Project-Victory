namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreateGameplayFeedbackHandler
{
    public GameplayFeedbackResult Handle(
        CreateGameplayFeedbackCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new GameplayFeedbackResult(
            command.CharacterId,
            command.Success,
            command.Success
                ? "Gameplay flow validated."
                : "Gameplay flow validation failed.");
    }
}