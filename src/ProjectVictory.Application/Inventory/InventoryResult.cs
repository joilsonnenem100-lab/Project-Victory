namespace ProjectVictory.Application.Inventory;

public sealed record InventoryResult(
    bool Success,
    IReadOnlyList<string> Items);
