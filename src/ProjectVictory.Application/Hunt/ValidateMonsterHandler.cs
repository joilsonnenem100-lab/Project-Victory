using System;

namespace ProjectVictory.Application.Hunt;

public sealed class ValidateMonsterHandler
{
    public ValidateMonsterResult Handle(
        ValidateMonsterCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.WorldId != Guid.Empty &&
            command.HuntAreaId != Guid.Empty &&
            command.EncounterId != Guid.Empty &&
            command.MonsterId != Guid.Empty &&
            command.CombatAvailable;

        return new ValidateMonsterResult(
            command.MonsterId,
            valid);
    }
}