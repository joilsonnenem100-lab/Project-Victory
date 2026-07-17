using System;

namespace ProjectVictory.Application.Session;

public sealed class ValidatePlayerSessionHandler
{
    public ValidatePlayerSessionResult Handle(
        ValidatePlayerSessionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.SessionId != Guid.Empty &&
            command.CharacterId != Guid.Empty &&
            command.GameplayActive &&
            command.PersistenceAvailable;

        return new ValidatePlayerSessionResult(
            command.SessionId,
            valid);
    }
}