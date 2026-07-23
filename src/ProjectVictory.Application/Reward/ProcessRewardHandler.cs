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


        var context =
            new RewardProcessingContext(
                command.CombatId,
                command.CharacterId);



        var rewardResult =
            new ProjectVictory.Application.Reward.RewardResult(
                0,
                0,
                []
            );



        return new ProcessRewardResult(
            context.CharacterId,

            true,

            rewardResult.Experience,

            rewardResult.Gold,

            rewardResult.Drops);
    }
}