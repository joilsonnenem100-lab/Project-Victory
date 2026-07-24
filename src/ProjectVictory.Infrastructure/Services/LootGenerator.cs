using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Reward;

namespace ProjectVictory.Infrastructure.Services;

public sealed class LootGenerator
{
    private readonly Random _random = new();

    public IReadOnlyList<RewardDrop> Generate(
        IReadOnlyList<LootDropDefinition> definitions)
    {
        var drops = new List<RewardDrop>();

        foreach (var definition in definitions)
        {
            if (_random.NextDouble() <= definition.Chance)
            {
                drops.Add(
                    new RewardDrop(
                        definition.Item.Id,
                        definition.Item.Name));
            }
        }

        return drops;
    }
}
