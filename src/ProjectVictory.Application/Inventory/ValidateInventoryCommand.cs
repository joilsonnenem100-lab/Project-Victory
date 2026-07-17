using System;

namespace ProjectVictory.Application.Inventory;

public sealed record ValidateInventoryCommand(
    Guid CharacterId);