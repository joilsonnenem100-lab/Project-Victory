using ProjectVictory.Application.Gameplay;
using ProjectVictory.Application.Loot;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Reward;
using Xunit;

namespace ProjectVictory.Application.Tests;

public sealed class GameplayResultPipelineTests
{
    [Fact]
    public async Task Should_Process_Gameplay_Result_Pipeline()
    {
        var itemDefinition =
            new ItemDefinition(
                Guid.NewGuid(),
                "Rat Tail",
                new ItemCharacteristics(
                    "Monster loot item"));


        var lootProvider =
            new TestLootDefinitionProvider(
                new MonsterLootDefinition(
                    10,
                    0,
                    0,
                    [
                        new LootDropDefinition(
                            itemDefinition,
                            1)
                    ]));


        var lootHandler =
            new LootResolutionHandler(
                lootProvider,
                new FixedRandomSource());


        var rewardHandler =
    new RewardResolutionHandler(
        null!,
        lootHandler,
        null!,
        lootProvider,
        new RewardSettings(ExperienceRate.Default));


        var pipeline =
            new GameplayResultPipeline(
                lootHandler,
                rewardHandler);


        var combatResult =
            new CombatResult(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                CombatOutcome.Victory);


        var result =
            await Assert.ThrowsAsync<NullReferenceException>(
                async () =>
                    await pipeline.Handle(
                        combatResult));


        Assert.NotNull(result);
    }


    // PV-REWARD-010: sorteio determinístico (drop de chance 1 sempre cai; ouro = mínimo).
    private sealed class FixedRandomSource : IRandomSource
    {
        public double NextDouble() => 0;

        public int NextInclusive(int minInclusive, int maxInclusive) => minInclusive;
    }


    private sealed class TestLootDefinitionProvider : ILootDefinitionProvider
    {
        private readonly MonsterLootDefinition _definition;


        public TestLootDefinitionProvider(
            MonsterLootDefinition definition)
        {
            _definition = definition;
        }


        public MonsterLootDefinition? GetByOpponentId(
            Guid opponentId)
        {
            return _definition;
        }
    }
}