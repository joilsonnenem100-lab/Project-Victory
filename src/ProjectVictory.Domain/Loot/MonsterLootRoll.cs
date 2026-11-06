using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Loot;

// PV-REWARD-010: sorteio do loot de um monstro a partir da definição (ADR-037: o Loot só gera
// a coleção; quem entrega é o Reward Pipeline).
public static class MonsterLootRoll
{
    public static int RollGold(
        MonsterLootDefinition definition,
        IRandomSource random)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(random);

        return random.NextInclusive(
            definition.MinimumGold,
            definition.MaximumGold);
    }

    // Cada drop cai com a sua chance: NextDouble() em [0, 1) < Chance.
    // Chance 1.0 sempre cai; 0 nunca cai.
    public static IReadOnlyList<ItemDefinition> RollDrops(
        MonsterLootDefinition definition,
        IRandomSource random)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(random);

        return definition.Drops
            .Where(drop => random.NextDouble() < drop.Chance)
            .Select(drop => drop.Item)
            .ToList();
    }
}
