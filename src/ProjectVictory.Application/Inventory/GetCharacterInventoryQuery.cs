using System;

namespace ProjectVictory.Application.Inventory;

public sealed record GetCharacterInventoryQuery(
    Guid CharacterId);