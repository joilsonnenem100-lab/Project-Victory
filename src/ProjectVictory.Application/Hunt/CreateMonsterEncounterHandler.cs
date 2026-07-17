using System;

namespace ProjectVictory.Application.Hunt;

public sealed class CreateMonsterEncounterHandler
{
    public CreateMonsterEncounterResult Handle(
        CreateMonsterEncounterCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var created =
            command.EncounterId != Guid.Empty &&
            command.MonsterId != Guid.Empty;

        return new CreateMonsterEncounterResult(
            command.EncounterId,
            command.MonsterId,
            created);
    }
}