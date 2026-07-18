using ProjectVictory.Application.Reward;

namespace ProjectVictory.Infrastructure.Services;

public sealed class RewardProcessor : IRewardProcessor
{
    public RewardResult Process(
        RewardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new RewardResult(
            0,
            0);
    }
}