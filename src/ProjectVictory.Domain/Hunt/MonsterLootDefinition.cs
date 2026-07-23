using ProjectVictory.Domain.Items;

namespace ProjectVictory.Domain.Hunt;

public sealed class MonsterLootDefinition
{
    public int Experience { get; }

    public int MinimumGold { get; }

    public int MaximumGold { get; }

    public IReadOnlyList<LootDropDefinition> Drops { get; }


    public MonsterLootDefinition(
        int experience,
        int minimumGold,
        int maximumGold,
        IReadOnlyList<LootDropDefinition> drops)
    {
        if (experience < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(experience));
        }


        if (minimumGold < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minimumGold));
        }


        if (maximumGold < minimumGold)
        {
            throw new ArgumentException(
                "Maximum gold cannot be lower than minimum gold.");
        }


        Experience = experience;

        MinimumGold = minimumGold;

        MaximumGold = maximumGold;

        Drops =
            drops ?? [];
    }
}



public sealed record LootDropDefinition(
    ItemDefinition Item,
    double Chance);