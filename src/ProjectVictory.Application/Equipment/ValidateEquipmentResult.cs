using System;

namespace ProjectVictory.Application.Equipment;

public sealed record ValidateEquipmentResult(
    Guid CharacterId,
    bool Valid);