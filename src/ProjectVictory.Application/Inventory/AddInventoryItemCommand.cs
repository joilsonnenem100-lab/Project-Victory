using System;

namespace ProjectVictory.Application.Inventory;

public sealed record AddInventoryItemCommand(
    Guid CharacterId,
    string Item);