using ProjectVictory.Application.Reward;

namespace ProjectVictory.Infrastructure.Services;

public class RewardProcessor : IRewardProcessor
{
    public RewardResult Process(
        RewardRequest request)
    {
        return new RewardResult(
            100,
            10
        );
    }
}