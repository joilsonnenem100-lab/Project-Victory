using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class CompletePlayerSessionLoopHandler
{
    public CompletePlayerSessionLoopResult Handle(
        CompletePlayerSessionLoopCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var completed =
            command.CharacterId != Guid.Empty &&
            command.RuntimeId != Guid.Empty &&
            command.SessionId != Guid.Empty;

        return new CompletePlayerSessionLoopResult(
            command.CharacterId,
            command.RuntimeId,
            command.SessionId,
            completed);
    }
}
