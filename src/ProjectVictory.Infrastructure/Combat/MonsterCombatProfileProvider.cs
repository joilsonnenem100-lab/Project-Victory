using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Infrastructure.Combat;

// PV-COMBAT-012: atributos de combate dos monstros, como dados.
// *** PROVISÓRIO *** (balanceamento do PO pendente; DEC-20261106-09). O PV-CONTENT-011 move esses
// dados para o catálogo da Spider Cave.
public sealed class MonsterCombatProfileProvider : IMonsterCombatProfileProvider
{
    private static readonly Guid RatId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid CaveRatId = Guid.Parse("10000000-0000-0000-0000-000000000002");

    // Monstro sem dados: perfil mínimo (PROVISÓRIO), para o combate nunca falhar por falta de dado.
    public static MonsterCombatDefinition Fallback { get; } =
        new(1, new CombatProfile(5, 5, 0, 0));

    private static readonly IReadOnlyDictionary<Guid, MonsterCombatDefinition> Definitions =
        new Dictionary<Guid, MonsterCombatDefinition>
        {
            [RatId] = new(1, new CombatProfile(10, 8, 1, 1)),
            [CaveRatId] = new(2, new CombatProfile(12, 9, 2, 2))
        };

    public MonsterCombatDefinition Get(Guid monsterId) =>
        Definitions.TryGetValue(monsterId, out var definition)
            ? definition
            : Fallback;
}
