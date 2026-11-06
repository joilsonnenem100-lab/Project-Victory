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



// PV-CONTENT-011 (pendência do M2): a chance de drop é validada entre 0 e 1.
public sealed record LootDropDefinition
{
    public ItemDefinition Item { get; }

    public double Chance { get; }

    public LootDropDefinition(
        ItemDefinition Item,
        double Chance)
    {
        ArgumentNullException.ThrowIfNull(Item);

        if (double.IsNaN(Chance) || Chance < 0 || Chance > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(Chance),
                "Drop chance must be between 0 and 1.");
        }

        this.Item = Item;
        this.Chance = Chance;
    }
}