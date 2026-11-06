using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Loot;

namespace ProjectVictory.Application.Loot;

public sealed class ApplyLootToCharacterInventoryHandler
{
    private readonly ICharacterRepository _characterRepository;


    public ApplyLootToCharacterInventoryHandler(
        ICharacterRepository characterRepository)
    {
        _characterRepository =
            characterRepository;
    }


    public async Task Handle(
        Guid characterId,
        LootResolutionResult lootResult,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            lootResult);


        if (lootResult.Items.Count == 0 && lootResult.Gold == 0)
        {
            return;
        }


        var character =
            await _characterRepository.GetByIdAsync(
                characterId,
                cancellationToken);


        if (character is null)
        {
            return;
        }


        foreach (var item in lootResult.Items)
        {
            character.Inventory.AddItem(
                item);
        }


        // PV-REWARD-010: o ouro do loot chega ao Inventory.
        if (lootResult.Gold > 0)
        {
            character.Inventory.AddGold(
                lootResult.Gold);
        }

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);
    }
}