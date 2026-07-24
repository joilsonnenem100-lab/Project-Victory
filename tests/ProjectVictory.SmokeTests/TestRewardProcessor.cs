using ProjectVictory.Application.Reward;

namespace ProjectVictory.SmokeTests;

public sealed class TestRewardProcessor : IRewardProcessor
{
    public RewardResult Process(
        RewardRequest request)
    {
        var drops = new List<RewardDrop>
        {
            new RewardDrop(
                Guid.NewGuid(),
                "Smoke Test Item")
        };

        return new RewardResult(
            50,
            1,
            drops);
    }
}