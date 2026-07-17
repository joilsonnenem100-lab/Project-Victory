using ProjectVictory.Domain.Reward;

namespace ProjectVictory.Application.Reward;

public sealed class ProcessRewardHandler
{
    public ProcessRewardResult Handle(
        ProcessRewardCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CombatId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CombatId is required.");
        }

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var context = new RewardProcessingContext(
            command.CombatId,
            command.CharacterId);

        ProjectVictory.Domain.Reward.RewardResult rewardResult =
            new ProjectVictory.Domain.Reward.RewardResult(
                context.CharacterId,
                context.CombatId,
                true);

        return new ProcessRewardResult(
            rewardResult.CharacterId,
            rewardResult.Processed);
    }
}