using System;

namespace ProjectVictory.Application.Reward;

public sealed class CreateRewardContentHandler
{
    public CreateRewardContentResult Handle(
        CreateRewardContentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var created =
            command.CombatResultId != Guid.Empty &&
            command.RewardContentId != Guid.Empty;

        return new CreateRewardContentResult(
            command.CombatResultId,
            command.RewardContentId,
            created);
    }
}