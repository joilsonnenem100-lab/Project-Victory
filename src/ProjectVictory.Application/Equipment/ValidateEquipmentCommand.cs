using System;

namespace ProjectVictory.Application.Equipment;

public sealed record ValidateEquipmentCommand(
    Guid CharacterId);