using System;

namespace ProjectVictory.Application.Combat;

public sealed class CreateCombatContentHandler
{
    public CreateCombatContentResult Handle(
        CreateCombatContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var created =
            command.EncounterId != Guid.Empty &&
            command.MonsterId != Guid.Empty &&
            command.CombatContentId != Guid.Empty;

        return new CreateCombatContentResult(
            command.EncounterId,
            command.CombatContentId,
            created);
    }
}