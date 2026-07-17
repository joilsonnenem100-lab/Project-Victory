using System;

namespace ProjectVictory.Application.Equipment;

public sealed record EquipItemResult(
    Guid CharacterId,
    string Item,
    bool Equipped);