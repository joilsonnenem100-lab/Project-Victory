using System;

namespace ProjectVictory.Application.Hunt;

public sealed class ValidateHuntAreaHandler
{
    public ValidateHuntAreaResult Handle(
        ValidateHuntAreaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.WorldId != Guid.Empty &&
            command.HuntAreaId != Guid.Empty &&
            command.HuntExecuted &&
            command.CombatCompleted &&
            command.RewardProcessed &&
            command.CharacterUpdated;

        return new ValidateHuntAreaResult(
            command.HuntAreaId,
            valid);
    }
}