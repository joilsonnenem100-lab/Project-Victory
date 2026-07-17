namespace ProjectVictory.Application.Hunt;

public sealed class ConfigureHuntContentHandler
{
    public ConfigureHuntContentResult Handle(
        ConfigureHuntContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.HuntAreaId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "HuntAreaId is required.");
        }

        if (command.EncounterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "EncounterId is required.");
        }

        return new ConfigureHuntContentResult(
            command.HuntAreaId,
            true);
    }
}