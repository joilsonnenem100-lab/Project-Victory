using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Equipment;

public sealed class RemoveEquipmentHandler
{
    private readonly ICharacterRepository _repository;


    public RemoveEquipmentHandler(
        ICharacterRepository repository)
    {
        _repository = repository;
    }


    public async Task<RemoveEquipmentResult> Handle(
        RemoveEquipmentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character =
            await _repository.GetByIdAsync(
                command.CharacterId);

        if (character is null)
        {
            return new RemoveEquipmentResult(
                command.CharacterId,
                command.Item,
                false);
        }

        character.Equipment.Remove(
            command.Item);

        await _repository.UpdateAsync(
            character);

        return new RemoveEquipmentResult(
            command.CharacterId,
            command.Item,
            true);
    }
}