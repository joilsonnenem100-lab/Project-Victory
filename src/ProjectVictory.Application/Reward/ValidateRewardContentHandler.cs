using System;

namespace ProjectVictory.Application.Reward;

public sealed class ValidateRewardContentHandler
{
    public ValidateRewardContentResult Handle(
        ValidateRewardContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.CombatResultId != Guid.Empty &&
            command.RewardContentId != Guid.Empty &&
            command.CharacterId != Guid.Empty &&
            command.RewardProcessed &&
            command.CharacterUpdated;

        return new ValidateRewardContentResult(
            command.RewardContentId,
            valid);
    }
}