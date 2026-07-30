using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Hunt;

public sealed class CompleteHuntRewardFlowHandler
{
    private readonly ForwardHuntRewardHandler _forwardHuntRewardHandler;

    private readonly ProjectVictory.Application.Reward.RewardResolutionHandler _rewardResolutionHandler;


    public CompleteHuntRewardFlowHandler(
        ForwardHuntRewardHandler forwardHuntRewardHandler,
        ProjectVictory.Application.Reward.RewardResolutionHandler rewardResolutionHandler)
    {
        _forwardHuntRewardHandler =
            forwardHuntRewardHandler;

        _rewardResolutionHandler =
            rewardResolutionHandler;
    }


    public async Task<ProjectVictory.Application.Reward.ResolvedReward> Handle(
        CombatResult combatResult,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            combatResult);


        await _forwardHuntRewardHandler.Handle(
            new ForwardHuntRewardCommand(
                combatResult.HuntId),
            cancellationToken);


        return await _rewardResolutionHandler.Handle(
    combatResult,
    cancellationToken);
    }
}