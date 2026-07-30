// src/ProjectVictory.Application/Loot/LootResolutionHandler.cs
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;

namespace ProjectVictory.Application.Loot;

public sealed class LootResolutionHandler
{
    private readonly ILootDefinitionProvider _lootDefinitionProvider;


    public LootResolutionHandler(
        ILootDefinitionProvider lootDefinitionProvider)
    {
        _lootDefinitionProvider =
            lootDefinitionProvider;
    }


    public LootResolutionResult Handle(
        CombatResult combatResult)
    {
        ArgumentNullException.ThrowIfNull(
            combatResult);


        if (combatResult.Outcome != CombatOutcome.Victory)
        {
            return new LootResolutionResult(
                []);
        }


        var lootDefinition =
            _lootDefinitionProvider.GetByOpponentId(
                combatResult.OpponentId);


        if (lootDefinition is null)
        {
            return new LootResolutionResult(
                []);
        }


        var items =
            lootDefinition.Drops
                .Select(drop =>
                    new ItemInstance(
                        Guid.NewGuid(),
                        drop.Item.Id))
                .ToList();


        return new LootResolutionResult(
            items);
    }
}