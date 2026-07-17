using System;

namespace ProjectVictory.Application.Persistence;

public sealed class ValidatePersistenceFinalHandler
{
    public ValidatePersistenceFinalResult Handle(
        ValidatePersistenceFinalCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var completed =
            command.CharacterId != Guid.Empty &&
            command.StatePersisted &&
            command.StateRecovered &&
            command.RuntimeRestored;

        return new ValidatePersistenceFinalResult(
            command.CharacterId,
            completed);
    }
}