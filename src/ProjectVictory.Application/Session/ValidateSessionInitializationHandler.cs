using System;

namespace ProjectVictory.Application.Session;

public sealed class ValidateSessionInitializationHandler
{
    public ValidateSessionInitializationResult Handle(
        ValidateSessionInitializationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.SessionId != Guid.Empty &&
            command.CharacterId != Guid.Empty;

        return new ValidateSessionInitializationResult(
            command.SessionId,
            valid);
    }
}