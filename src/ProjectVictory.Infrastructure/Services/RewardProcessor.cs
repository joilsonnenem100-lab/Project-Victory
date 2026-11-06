using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Infrastructure.Services;

// PV-REWARD-010: deixa de ser stub. Para uma vitória em combate (SourceResult = CombatResult),
// XP e ouro vêm da definição do monstro e os drops respeitam a chance da Loot Table.
// Devolve a XP BASE: o XP rate é aplicado pelo Reward Pipeline (ApplyRewardHandler).
// Qualquer outra origem não gera recompensa (antes: item fictício "Smoke Test Item").
public class RewardProcessor : IRewardProcessor
{
    private readonly ILootDefinitionProvider _lootDefinitionProvider;
    private readonly IRandomSource _random;

    public RewardProcessor(
        ILootDefinitionProvider lootDefinitionProvider,
        IRandomSource random)
    {
        _lootDefinitionProvider = lootDefinitionProvider;
        _random = random;
    }

    public RewardResult Process(
        RewardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceResult is not CombatResult { Outcome: CombatOutcome.Victory } combatResult)
        {
            return new RewardResult(0, 0, []);
        }

        var definition =
            _lootDefinitionProvider.GetByOpponentId(
                combatResult.OpponentId);

        if (definition is null)
        {
            return new RewardResult(0, 0, []);
        }

        var drops =
            MonsterLootRoll.RollDrops(definition, _random)
                .Select(item => new RewardDrop(item.Id, item.Name))
                .ToList();

        return new RewardResult(
            definition.Experience,
            MonsterLootRoll.RollGold(definition, _random),
            drops);
    }
}
