using System;

namespace ProjectVictory.Application.Session;

public sealed class ValidateSessionFinalizationHandler
{
    public ValidateSessionFinalizationResult Handle(
        ValidateSessionFinalizationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var approved =
            command.SessionId != Guid.Empty &&
            command.FinalStateValid &&
            command.PersistenceValidated;

        return new ValidateSessionFinalizationResult(
            command.SessionId,
            approved);
    }
}