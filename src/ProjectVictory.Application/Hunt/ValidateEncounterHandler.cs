using System;

namespace ProjectVictory.Application.Hunt;

public sealed class ValidateEncounterHandler
{
    public ValidateEncounterResult Handle(
        ValidateEncounterCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.WorldId != Guid.Empty &&
            command.HuntAreaId != Guid.Empty &&
            command.EncounterId != Guid.Empty &&
            command.HuntAvailable &&
            command.CombatAvailable;

        return new ValidateEncounterResult(
            command.EncounterId,
            valid);
    }
}