using ProjectVictory.Domain.Items;

namespace ProjectVictory.Domain.Loot;

public sealed class LootResolutionResult
{
    public IReadOnlyList<ItemInstance> Items { get; }


    // PV-REWARD-010: ouro sorteado da definição do monstro.
    public int Gold { get; }


    public LootResolutionResult(
        IReadOnlyList<ItemInstance> items)
        : this(items, 0)
    {
    }


    public LootResolutionResult(
        IReadOnlyList<ItemInstance> items,
        int gold)
    {
        if (gold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gold));
        }

        Items =
            items ?? [];

        Gold = gold;
    }
}