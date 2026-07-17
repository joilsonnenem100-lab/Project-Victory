using System;

namespace ProjectVictory.Application.Inventory;

public sealed record AddDefinedItemResult(
    Guid CharacterId,
    string ItemId,
    bool Added);