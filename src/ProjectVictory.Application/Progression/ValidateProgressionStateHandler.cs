namespace ProjectVictory.Application.Progression;

public sealed class ValidateProgressionStateHandler
{
    public ValidateProgressionStateResult Handle(
        ValidateProgressionStateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ValidateProgressionStateResult(
            command.CharacterId,
            true);
    }
}