using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Infrastructure.Content;

namespace ProjectVictory.Infrastructure.Combat;

// PV-CONTENT-011: atributos de combate vêm do catálogo da Spider Cave (dados PROVISÓRIOS).
public sealed class MonsterCombatProfileProvider : IMonsterCombatProfileProvider
{
    // Monstro sem dados no catálogo: perfil mínimo (PROVISÓRIO), para o combate não falhar.
    // Todos os monstros do jogo hoje estão no catálogo; um teste garante isso (revisão do 012).
    public static MonsterCombatDefinition Fallback { get; } =
        new(1, new CombatProfile(5, 5, 0, 0));

    public MonsterCombatDefinition Get(Guid monsterId)
    {
        var monster = SpiderCaveCatalog.Find(monsterId);

        return monster is null
            ? Fallback
            : new MonsterCombatDefinition(monster.Level, monster.Combat);
    }
}
