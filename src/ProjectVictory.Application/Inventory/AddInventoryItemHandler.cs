using System;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Inventory;

public sealed class AddInventoryItemHandler
{
    private readonly ICharacterRepository _repository;

    public AddInventoryItemHandler(ICharacterRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddInventoryItemResult> Handle(AddInventoryItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character = await _repository.GetByIdAsync(command.CharacterId);

        if (character is null || !Guid.TryParse(command.Item, out var itemDefinitionId))
        {
            return new AddInventoryItemResult(command.CharacterId, command.Item, false);
        }

        character.Inventory.AddItem(
            new ItemInstance(Guid.NewGuid(), itemDefinitionId));

        await _repository.UpdateAsync(character);

        return new AddInventoryItemResult(command.CharacterId, command.Item, true);
    }
}
