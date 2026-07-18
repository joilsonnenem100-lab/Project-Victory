using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ExecuteCompleteGameplayLoopHandler
{
    public ExecuteCompleteGameplayLoopResult Handle(
        ExecuteCompleteGameplayLoopCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var completed =
            command.CharacterId != Guid.Empty &&
            command.SessionId != Guid.Empty;

        return new ExecuteCompleteGameplayLoopResult(
            command.CharacterId,
            completed);
    }
}