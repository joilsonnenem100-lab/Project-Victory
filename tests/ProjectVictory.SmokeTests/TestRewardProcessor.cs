using ProjectVictory.Application.Reward;

namespace ProjectVictory.SmokeTests;

public sealed class TestRewardProcessor : IRewardProcessor
{
    // PV-REWARD-010: id fixo para o teste conferir o ItemDefinitionId do drop.
    public static readonly Guid DropItemDefinitionId =
        Guid.Parse("00000000-0000-0000-0000-00000000a001");

    public const int Experience = 50;

    public const int Gold = 1;

    public RewardResult Process(
        RewardRequest request)
    {
        var drops = new List<RewardDrop>
        {
            new RewardDrop(
                DropItemDefinitionId,
                "Smoke Test Item")
        };

        return new RewardResult(
            Experience,
            Gold,
            drops);
    }
}