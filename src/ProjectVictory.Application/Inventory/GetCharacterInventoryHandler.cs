using ProjectVictory.Application.Session;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Application.Inventory;

public sealed class GetCharacterInventoryHandler
{
    private readonly ICurrentPlayerContext _currentPlayerContext;


    public GetCharacterInventoryHandler(
        ICurrentPlayerContext currentPlayerContext)
    {
        _currentPlayerContext = currentPlayerContext;
    }


    public InventoryResult Handle(
        GetCharacterInventoryQuery query,
        CharacterEntity character)
    {
        // PV-SEC-001: só o dono do personagem consulta o inventário.
        if (character.AccountId.Value != _currentPlayerContext.AccountId)
        {
            throw new OwnershipViolationException(
                "character inventory");
        }

        if (character.Id != query.CharacterId)
        {
            return new InventoryResult(false, []);
        }

        var items = character.Inventory.Items
            .Select(x => x.Id.ToString())
            .ToList();

        return new InventoryResult(true, items);
    }
}
