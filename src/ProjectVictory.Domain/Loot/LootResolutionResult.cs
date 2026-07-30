using ProjectVictory.Domain.Items;

namespace ProjectVictory.Domain.Loot;

public sealed class LootResolutionResult
{
    public IReadOnlyList<ItemInstance> Items { get; }


    public LootResolutionResult(
        IReadOnlyList<ItemInstance> items)
    {
        Items =
            items ?? [];
    }
}