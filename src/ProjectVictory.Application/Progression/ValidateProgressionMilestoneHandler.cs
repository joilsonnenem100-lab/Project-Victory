using System;

namespace ProjectVictory.Application.Progression;

public sealed class ValidateProgressionMilestoneHandler
{
    public ValidateProgressionMilestoneResult Handle(
        ValidateProgressionMilestoneCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CharacterId != Guid.Empty &&
            command.ProgressionProcessed &&
            command.CharacterUpdated;

        return new ValidateProgressionMilestoneResult(
            command.CharacterId,
            valid);
    }
}