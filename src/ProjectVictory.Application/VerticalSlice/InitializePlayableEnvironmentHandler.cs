using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class InitializePlayableEnvironmentHandler
{
    public InitializePlayableEnvironmentResult Handle(
        InitializePlayableEnvironmentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var initialized =
            command.RuntimeId != Guid.Empty &&
            command.CharacterId != Guid.Empty;

        return new InitializePlayableEnvironmentResult(
            command.RuntimeId,
            initialized);
    }
}