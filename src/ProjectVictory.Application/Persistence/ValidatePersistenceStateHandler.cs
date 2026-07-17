using System;

namespace ProjectVictory.Application.Persistence;

public sealed class ValidatePersistenceStateHandler
{
    public ValidatePersistenceStateResult Handle(
        ValidatePersistenceStateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.StateId != Guid.Empty &&
            command.StateRepresentable;

        return new ValidatePersistenceStateResult(
            command.StateId,
            valid);
    }
}