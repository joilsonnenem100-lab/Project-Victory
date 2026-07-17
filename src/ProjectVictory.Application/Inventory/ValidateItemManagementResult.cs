using System;

namespace ProjectVictory.Application.Inventory;

public sealed record ValidateItemManagementResult(
    Guid CharacterId,
    bool Valid);