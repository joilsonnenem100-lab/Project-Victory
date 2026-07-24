using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Application.Inventory;

public sealed class GetCharacterInventoryHandler
{
    public InventoryResult Handle(
        GetCharacterInventoryQuery query,
        CharacterEntity character)
    {
        if (character.Id != query.CharacterId)
        {
            return new InventoryResult(
                false,
                []);
        }

        var items = character.Inventory.Items
            .ToList();

        return new InventoryResult(
            true,
            items);
    }
}