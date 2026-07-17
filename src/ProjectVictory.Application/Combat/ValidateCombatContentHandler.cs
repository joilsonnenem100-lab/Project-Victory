using System;

namespace ProjectVictory.Application.Combat;

public sealed class ValidateCombatContentHandler
{
    public ValidateCombatContentResult Handle(
        ValidateCombatContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.WorldId != Guid.Empty &&
            command.EncounterId != Guid.Empty &&
            command.MonsterId != Guid.Empty &&
            command.CombatContentId != Guid.Empty &&
            command.CombatExecuted &&
            command.ResultGenerated;

        return new ValidateCombatContentResult(
            command.CombatContentId,
            valid);
    }
}