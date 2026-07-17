using System;

namespace ProjectVictory.Application.Inventory;

public sealed record ValidateItemManagementCommand(
    Guid CharacterId);