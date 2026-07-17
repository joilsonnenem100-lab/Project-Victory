using System;

namespace ProjectVictory.Application.World;

public sealed class ValidateWorldContentHandler
{
    public ValidateWorldContentResult Handle(
        ValidateWorldContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.WorldId != Guid.Empty &&
            command.ContentLoaded &&
            command.HuntAvailable;

        return new ValidateWorldContentResult(
            command.WorldId,
            valid);
    }
}