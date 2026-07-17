using System;

namespace ProjectVictory.Application.Equipment;

public sealed record EquipDefinedItemCommand(
    Guid CharacterId,
    string ItemId);