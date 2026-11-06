namespace ProjectVictory.Persistence.Item;

public sealed class PersistentItemState
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // PV-REWARD-010: preço de venda ao NPC (DEC-20261106-08), provisório.
    public int NpcSellPrice { get; set; }
}