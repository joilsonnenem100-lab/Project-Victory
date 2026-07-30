namespace ProjectVictory.Application.Equipment;

public sealed record EquipDefinedItemCommand(
    Guid CharacterId,
    Guid ItemId);
