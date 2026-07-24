using ProjectVictory.Application.Reward;

namespace ProjectVictory.Infrastructure.Services;

public class RewardProcessor : IRewardProcessor
{
    public RewardResult Process(
        RewardRequest request)
    {
        var drops =
            new List<RewardDrop>
            {
                new RewardDrop(
                    Guid.NewGuid(),
                    "Smoke Test Item")
            };

        return new RewardResult(
            0,
            0,
            drops);
    }
}