using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Equipment;

public sealed class EquipItemHandler
{
    private readonly ICharacterRepository _repository;


    public EquipItemHandler(
        ICharacterRepository repository)
    {
        _repository = repository;
    }


    public async Task<EquipItemResult> Handle(
        EquipItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character =
            await _repository.GetByIdAsync(
                command.CharacterId);

        if (character is null)
        {
            return new EquipItemResult(
                command.CharacterId,
                command.Item,
                false);
        }

        character.Equipment.Equip(
            command.Item);

        await _repository.UpdateAsync(
            character);

        return new EquipItemResult(
            command.CharacterId,
            command.Item,
            true);
    }
}