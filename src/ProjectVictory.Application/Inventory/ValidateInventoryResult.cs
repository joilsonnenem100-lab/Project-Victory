using System;

namespace ProjectVictory.Application.Inventory;

public sealed record ValidateInventoryResult(
    Guid CharacterId,
    bool Valid);