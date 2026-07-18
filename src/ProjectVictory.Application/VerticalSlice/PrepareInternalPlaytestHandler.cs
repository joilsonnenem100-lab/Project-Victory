using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class PrepareInternalPlaytestHandler
{
    public PrepareInternalPlaytestResult Handle(
        PrepareInternalPlaytestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var prepared =
            command.RuntimeId != Guid.Empty &&
            command.CharacterId != Guid.Empty &&
            command.EnvironmentReady;

        return new PrepareInternalPlaytestResult(
            command.RuntimeId,
            prepared);
    }
}