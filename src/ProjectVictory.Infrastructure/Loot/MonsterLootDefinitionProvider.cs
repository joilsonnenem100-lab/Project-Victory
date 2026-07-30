using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;

namespace ProjectVictory.Infrastructure.Loot;

public sealed class MonsterLootDefinitionProvider : ILootDefinitionProvider
{
    private readonly IReadOnlyList<MonsterDefinition> _monsters =
    [
        new MonsterDefinition(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "Rat",
            Guid.Empty,
            new MonsterLootDefinition(
                10,
                1,
                5,
                [
                    new LootDropDefinition(
                        new ItemDefinition(
                            Guid.Parse("00000000-0000-0000-0000-000000000001"),
                            "Rat Tail",
                            new ItemCharacteristics(
                                "Validation loot item")),
                        1.0)
                ]))
    ];


    public MonsterLootDefinition? GetByOpponentId(
        Guid opponentId)
    {
        var monster =
            _monsters.FirstOrDefault(
                x => x.Id == opponentId);


        return monster?.Loot;
    }
}