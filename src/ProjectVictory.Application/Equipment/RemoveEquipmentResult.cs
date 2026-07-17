using System;

namespace ProjectVictory.Application.Equipment;

public sealed record RemoveEquipmentResult(
    Guid CharacterId,
    string Item,
    bool Removed);