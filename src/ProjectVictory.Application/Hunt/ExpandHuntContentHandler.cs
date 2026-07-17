namespace ProjectVictory.Application.Hunt;

public sealed class ExpandHuntContentHandler
{
    public ExpandHuntContentResult Handle(
        ExpandHuntContentCommand command)
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

        return new ExpandHuntContentResult(
            command.HuntAreaId,
            true);
    }
}