using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Inventory;

public sealed class AddDefinedItemHandler
{
    private readonly ICharacterRepository _repository;

    public AddDefinedItemHandler(ICharacterRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddDefinedItemResult> Handle(AddDefinedItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var character = await _repository.GetByIdAsync(command.CharacterId);

        if (character is null)
        {
            return new AddDefinedItemResult(command.CharacterId, command.ItemId, false);
        }

        if (!Guid.TryParse(command.ItemId, out var itemDefinitionId))
        {
            return new AddDefinedItemResult(command.CharacterId, command.ItemId, false);
        }

        character.Inventory.AddItem(
            new ItemInstance(
                Guid.NewGuid(),
                itemDefinitionId));

        await _repository.UpdateAsync(character);

        return new AddDefinedItemResult(command.CharacterId, command.ItemId, true);
    }
}
