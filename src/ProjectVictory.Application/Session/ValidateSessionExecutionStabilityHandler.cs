using System;

namespace ProjectVictory.Application.Session;

public sealed class ValidateSessionExecutionStabilityHandler
{
    public ValidateSessionExecutionStabilityResult Handle(
        ValidateSessionExecutionStabilityCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.SessionId != Guid.Empty &&
            command.Active &&
            command.StatePreserved;

        return new ValidateSessionExecutionStabilityResult(
            command.SessionId,
            valid);
    }
}