namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateRuntimeInitializationHandler
{
    public ValidateRuntimeInitializationResult Handle(
        ValidateRuntimeInitializationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ValidateRuntimeInitializationResult(
            command.CharacterId,
            true);
    }
}