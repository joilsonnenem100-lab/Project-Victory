using System;

namespace ProjectVictory.Application.Equipment;

public sealed record EquipItemCommand(
    Guid CharacterId,
    string Item);