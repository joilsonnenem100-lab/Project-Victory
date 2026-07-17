using System;

namespace ProjectVictory.Application.Progression;

public sealed class ValidateCharacterGrowthHandler
{
    public ValidateCharacterGrowthResult Handle(
        ValidateCharacterGrowthCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.ProgressionProcessed &&
            command.CharacterUpdated;

        return new ValidateCharacterGrowthResult(
            command.CharacterId,
            valid);
    }
}