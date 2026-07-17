namespace ProjectVictory.Application.Hunt;

public sealed class PrepareHuntContentHandler
{
    public PrepareHuntContentResult Handle(
        PrepareHuntContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.HuntContentId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "HuntContentId is required.");
        }

        return new PrepareHuntContentResult(
            command.HuntContentId,
            true);
    }
}