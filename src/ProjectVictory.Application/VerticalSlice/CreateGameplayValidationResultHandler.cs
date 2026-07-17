namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreateGameplayValidationResultHandler
{
    public GameplayValidationResult Handle(
        CreateGameplayValidationResultCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var completed =
            command.SessionValid &&
            command.HuntValid &&
            command.CombatValid &&
            command.RewardValid &&
            command.ProgressionValid;

        return new GameplayValidationResult(
            command.CharacterId,
            command.SessionValid,
            command.HuntValid,
            command.CombatValid,
            command.RewardValid,
            command.ProgressionValid,
            completed);
    }
}