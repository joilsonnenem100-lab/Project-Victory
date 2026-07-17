using System;

namespace ProjectVictory.Application.Equipment;

public sealed record EquipDefinedItemResult(
    Guid CharacterId,
    string ItemId,
    bool Equipped);