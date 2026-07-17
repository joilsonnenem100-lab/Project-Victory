namespace ProjectVictory.Application.Progression;

public sealed class ValidateProgressionFlowHandler
{
    public ValidateProgressionFlowResult Handle(
        ValidateProgressionFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ValidateProgressionFlowResult(
            command.CharacterId,
            command.CharacterApplied);
    }
}