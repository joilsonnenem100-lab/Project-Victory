using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Inventory;

public sealed class AddDefinedItemHandler
{
    private readonly ICharacterRepository _repository;


    public AddDefinedItemHandler(
        ICharacterRepository repository)
    {
        _repository = repository;
    }


    public async Task<AddDefinedItemResult> Handle(
        AddDefinedItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character =
            await _repository.GetByIdAsync(
                command.CharacterId);

        if (character is null)
        {
            return new AddDefinedItemResult(
                command.CharacterId,
                command.ItemId,
                false);
        }

        character.Inventory.AddItem(
            command.ItemId);

        await _repository.UpdateAsync(
            character);

        return new AddDefinedItemResult(
            command.CharacterId,
            command.ItemId,
            true);
    }
}