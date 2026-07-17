using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateRuntimeExecutionHandler
{
    public ValidateRuntimeExecutionResult Handle(
        ValidateRuntimeExecutionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.RuntimeId != Guid.Empty &&
            command.CharacterId != Guid.Empty &&
            command.Initialized &&
            command.GameplayExecuted &&
            command.StateRecovered;

        return new ValidateRuntimeExecutionResult(
            command.RuntimeId,
            valid);
    }
}