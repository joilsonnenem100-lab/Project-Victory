using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Infrastructure.Content;

namespace ProjectVictory.Infrastructure.Combat;

// PV-MONSTER-010 (ADR-005): o comportamento vem da definição de dados do monstro (catálogo da
// Spider Cave). Monstro fora do catálogo: só ataque básico (mesmo critério do MonsterCombatProfileProvider).
public sealed class MonsterBehaviorProvider : IMonsterBehaviorProvider
{
    public MonsterBehavior Get(Guid monsterId) =>
        SpiderCaveCatalog.Find(monsterId)?.Behavior ?? MonsterBehavior.BasicAttackOnly;
}
