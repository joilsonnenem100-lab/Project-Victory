using System;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Equipment;

public sealed class EquipDefinedItemHandler
{
    private readonly ICharacterRepository _repository;

    public EquipDefinedItemHandler(ICharacterRepository repository)
    {
        _repository = repository;
    }

    public async Task<EquipDefinedItemResult> Handle(
        EquipDefinedItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character =
            await _repository.GetByIdAsync(command.CharacterId);

        if (character is null)
        {
            return new EquipDefinedItemResult(
                command.CharacterId,
                command.ItemId.ToString(),
                false);
        }

        if (!character.Inventory.Contains(command.ItemId))
        {
            return new EquipDefinedItemResult(
                command.CharacterId,
                command.ItemId.ToString(),
                false);
        }

        character.Equipment.Equip(
            command.ItemId.ToString());

        await _repository.UpdateAsync(character);

        return new EquipDefinedItemResult(
            command.CharacterId,
            command.ItemId.ToString(),
            true);
    }
}
