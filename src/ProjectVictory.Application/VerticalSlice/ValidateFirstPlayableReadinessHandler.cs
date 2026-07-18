using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateFirstPlayableReadinessHandler
{
    public ValidateFirstPlayableReadinessResult Handle(
        ValidateFirstPlayableReadinessCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ready =
            command.CharacterId != Guid.Empty &&
            command.GameplayReady &&
            command.RuntimeReady &&
            command.PersistenceReady;

        return new ValidateFirstPlayableReadinessResult(
            command.CharacterId,
            ready);
    }
}