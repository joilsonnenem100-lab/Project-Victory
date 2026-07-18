using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateFirstPlayableExecutionHandler
{
    public ValidateFirstPlayableExecutionResult Handle(
        ValidateFirstPlayableExecutionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ready =
            command.CharacterId != Guid.Empty &&
            command.EnvironmentInitialized &&
            command.GameplayExecuted &&
            command.StatePersisted;

        return new ValidateFirstPlayableExecutionResult(
            command.CharacterId,
            ready);
    }
}