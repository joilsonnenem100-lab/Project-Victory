using ProjectVictory.Domain.Loot;
using ProjectVictory.Application.Reward;

namespace ProjectVictory.Application.Gameplay;

public sealed class GameplayResultPipelineResult
{
    public LootResolutionResult Loot { get; }

    public ResolvedReward Reward { get; }


    public GameplayResultPipelineResult(
        LootResolutionResult loot,
        ResolvedReward reward)
    {
        Loot =
            loot;

        Reward =
            reward;
    }
}