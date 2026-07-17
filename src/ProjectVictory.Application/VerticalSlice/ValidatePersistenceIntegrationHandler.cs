namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidatePersistenceIntegrationHandler
{
    public ValidatePersistenceIntegrationResult Handle(
        ValidatePersistenceIntegrationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ValidatePersistenceIntegrationResult(
            command.CharacterId,
            true);
    }
}