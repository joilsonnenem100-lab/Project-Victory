using System;

namespace ProjectVictory.Application.Equipment;

public sealed record RemoveEquipmentCommand(
    Guid CharacterId,
    string Item);