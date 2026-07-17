using System;

namespace ProjectVictory.Application.Progression;

public sealed class ValidateAdvancedProgressionHandler
{
    public ValidateAdvancedProgressionResult Handle(
        ValidateAdvancedProgressionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.ProgressionProcessed &&
            command.CharacterUpdated &&
            command.PersistenceUpdated;

        return new ValidateAdvancedProgressionResult(
            command.CharacterId,
            valid);
    }
}