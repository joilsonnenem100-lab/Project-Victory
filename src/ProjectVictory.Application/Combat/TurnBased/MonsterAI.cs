using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-MONSTER-010 (ADR-005): fonte dos dados de comportamento de cada monstro (definição de dados).
public interface IMonsterBehaviorProvider
{
    MonsterBehavior Get(Guid monsterId);
}

// Sem catálogo (testes de unidade): todos os monstros só com o ataque básico.
public sealed class BasicAttackOnlyBehaviorProvider : IMonsterBehaviorProvider
{
    public static BasicAttackOnlyBehaviorProvider Instance { get; } = new();

    public MonsterBehavior Get(Guid monsterId) => MonsterBehavior.BasicAttackOnly;
}

// PV-MONSTER-010 (ADR-005/006): Monster AI — escolhe a Secondary Action de um monstro.
public interface IMonsterAI
{
    CombatTurnChoice? SelectSecondaryChoice(CombatSession session, Combatant monster);
}

// PV-MONSTER-010: a ÚNICA implementação de IA da engine (ADR-005). Não conhece espécies: lê as regras
// de dados do monstro (pelo SourceId = MonsterId) e sorteia com a aleatoriedade do encounter
// (session.Random, semente do encounter — determinística). Sem regras, devolve null SEM sortear.
public sealed class DataDrivenMonsterAI : IMonsterAI
{
    private readonly IMonsterBehaviorProvider _behaviors;

    public DataDrivenMonsterAI(IMonsterBehaviorProvider behaviors)
    {
        _behaviors = behaviors ?? throw new ArgumentNullException(nameof(behaviors));
    }

    public CombatTurnChoice? SelectSecondaryChoice(CombatSession session, Combatant monster)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(monster);

        if (monster.Side != CombatSide.Monster)
        {
            return null;
        }

        foreach (var rule in _behaviors.Get(monster.SourceId).Rules)
        {
            if (session.Random.NextInclusive(1, 100) <= rule.ChancePercent)
            {
                return new CombatTurnChoice(rule.Kind, rule.Skill);
            }
        }

        return null;
    }
}
