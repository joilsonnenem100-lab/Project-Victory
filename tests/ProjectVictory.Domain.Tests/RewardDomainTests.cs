using ProjectVictory.Domain.Entities;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Reward;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-REWARD-010: XP rate, ouro no Inventory, sorteio de loot e preço de venda.
public sealed class RewardDomainTests
{
    private sealed class FixedRandomSource(double nextDouble, int pick) : IRandomSource
    {
        public double NextDouble() => nextDouble;

        public int NextInclusive(int minInclusive, int maxInclusive) =>
            Math.Clamp(pick, minInclusive, maxInclusive);
    }

    private static ItemDefinition Item(string name, int price = 0) =>
        new(Guid.NewGuid(), name, new ItemCharacteristics(name), price);

    [Theory]
    [InlineData("1", 10L, 10L)]
    [InlineData("2", 10L, 20L)]
    [InlineData("1.5", 15L, 22L)]
    [InlineData("0.5", 3L, 1L)]
    public void ExperienceRate_AppliesAndFloors(string rate, long baseXp, long expected)
    {
        Assert.Equal(expected, new ExperienceRate(decimal.Parse(rate, System.Globalization.CultureInfo.InvariantCulture)).Apply(baseXp));
    }

    [Fact]
    public void ExperienceRate_DefaultIsOneX_AndRejectsZeroOrNegative()
    {
        Assert.Equal(1m, ExperienceRate.Default.Value);
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExperienceRate(0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExperienceRate(-1m));
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceRate.Default.Apply(-1));
    }

    [Fact]
    public void Inventory_Gold_StartsAtZero_AndOnlyGrows()
    {
        var inventory = new Inventory();

        inventory.AddGold(5);
        inventory.AddGold(0);

        Assert.Equal(5, inventory.Gold);
        Assert.Throws<ArgumentOutOfRangeException>(() => inventory.AddGold(-1));
    }

    [Fact]
    public void ItemDefinition_SellPrice_DefaultsToZero_AndRejectsNegative()
    {
        Assert.Equal(0, new ItemDefinition(Guid.NewGuid(), "A", new ItemCharacteristics("a")).NpcSellPrice);
        Assert.Equal(2, Item("Rat Tail", 2).NpcSellPrice);
        Assert.Throws<ArgumentOutOfRangeException>(() => Item("B", -1));
    }

    [Fact]
    public void MonsterLootRoll_RespectsChance()
    {
        var always = Item("Always");
        var half = Item("Half");
        var never = Item("Never");
        var definition = new MonsterLootDefinition(10, 1, 5,
        [
            new LootDropDefinition(always, 1.0),
            new LootDropDefinition(half, 0.5),
            new LootDropDefinition(never, 0.0)
        ]);

        var lowRoll = MonsterLootRoll.RollDrops(definition, new FixedRandomSource(0.2, 0));
        var highRoll = MonsterLootRoll.RollDrops(definition, new FixedRandomSource(0.7, 0));

        Assert.Equal(new[] { always, half }, lowRoll);
        Assert.Equal(new[] { always }, highRoll);
    }

    [Fact]
    public void MonsterLootRoll_GoldIsWithinDefinitionRange()
    {
        var definition = new MonsterLootDefinition(10, 1, 5, []);

        Assert.Equal(1, MonsterLootRoll.RollGold(definition, new FixedRandomSource(0, int.MinValue)));
        Assert.Equal(5, MonsterLootRoll.RollGold(definition, new FixedRandomSource(0, int.MaxValue)));
    }
}
