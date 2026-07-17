using System;

namespace ProjectVictory.Application.World;

public sealed class CreateWorldContentHandler
{
    public CreateWorldContentResult Handle(
        CreateWorldContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var created =
            command.WorldId != Guid.Empty &&
            command.LocationId != Guid.Empty;

        return new CreateWorldContentResult(
            command.WorldId,
            command.LocationId,
            created);
    }
}