using System;

namespace ProjectVictory.Application.Inventory;

public sealed record AddDefinedItemCommand(
    Guid CharacterId,
    string ItemId);