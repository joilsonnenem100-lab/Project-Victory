using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Equipment;

public sealed class EquipDefinedItemHandler
{
    private readonly ICharacterRepository _repository;


    public EquipDefinedItemHandler(
        ICharacterRepository repository)
    {
        _repository = repository;
    }


    public async Task<EquipDefinedItemResult> Handle(
        EquipDefinedItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character =
            await _repository.GetByIdAsync(
                command.CharacterId);

        if (character is null)
        {
            return new EquipDefinedItemResult(
                command.CharacterId,
                command.ItemId,
                false);
        }

        if (!character.Inventory.Contains(
                command.ItemId))
        {
            return new EquipDefinedItemResult(
                command.CharacterId,
                command.ItemId,
                false);
        }

        character.Equipment.Equip(
            command.ItemId);

        await _repository.UpdateAsync(
            character);

        return new EquipDefinedItemResult(
            command.CharacterId,
            command.ItemId,
            true);
    }
}