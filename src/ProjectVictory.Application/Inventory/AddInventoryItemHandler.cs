using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Inventory;

public sealed class AddInventoryItemHandler
{
    private readonly ICharacterRepository _repository;

    public AddInventoryItemHandler(
        ICharacterRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddInventoryItemResult> Handle(
        AddInventoryItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character =
            await _repository.GetByIdAsync(
                command.CharacterId);

        if (character is null)
        {
            return new AddInventoryItemResult(
                command.CharacterId,
                command.Item,
                false);
        }

        character.Inventory.AddItem(
            command.Item);

        await _repository.UpdateAsync(
            character);

        return new AddInventoryItemResult(
            command.CharacterId,
            command.Item,
            true);
    }
}