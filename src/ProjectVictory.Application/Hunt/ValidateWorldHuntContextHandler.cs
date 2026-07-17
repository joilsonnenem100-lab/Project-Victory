using System;

namespace ProjectVictory.Application.Hunt;

public sealed class ValidateWorldHuntContextHandler
{
    public ValidateWorldHuntContextResult Handle(
        ValidateWorldHuntContextCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.WorldId != Guid.Empty &&
            command.LocationId != Guid.Empty &&
            command.HuntId != Guid.Empty;

        return new ValidateWorldHuntContextResult(
            command.HuntId,
            valid);
    }
}