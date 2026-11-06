// src/ProjectVictory.Application/Loot/LootResolutionHandler.cs
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Application.Loot;

public sealed class LootResolutionHandler
{
    private readonly ILootDefinitionProvider _lootDefinitionProvider;
    private readonly IRandomSource _random;


    public LootResolutionHandler(
        ILootDefinitionProvider lootDefinitionProvider,
        IRandomSource random)
    {
        _lootDefinitionProvider =
            lootDefinitionProvider;

        _random =
            random;
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


        // PV-REWARD-010: cada drop respeita a chance da Loot Table (antes: todos caíam sempre)
        // e o ouro é sorteado entre o mínimo e o máximo da definição do monstro.
        var items =
            MonsterLootRoll.RollDrops(
                    lootDefinition,
                    _random)
                .Select(item =>
                    new ItemInstance(
                        Guid.NewGuid(),
                        item.Id))
                .ToList();


        return new LootResolutionResult(
            items,
            MonsterLootRoll.RollGold(
                lootDefinition,
                _random));
    }
}
