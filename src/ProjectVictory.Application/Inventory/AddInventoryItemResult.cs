using System;

namespace ProjectVictory.Application.Inventory;

public sealed record AddInventoryItemResult(
    Guid CharacterId,
    string Item,
    bool Added);