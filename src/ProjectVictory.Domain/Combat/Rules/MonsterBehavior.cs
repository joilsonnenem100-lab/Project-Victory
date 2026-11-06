using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Domain.Combat.Rules;

// PV-MONSTER-010 (ADR-005): o comportamento de cada espécie é DADO — a lista de habilidades que ela
// pode usar como Secondary Action (ADR-006: o Basic Attack é sempre executado pela Combat Engine; a
// Monster AI só escolhe a Secondary Action). A engine tem UMA implementação de IA, que lê estes dados.
//
// Uma regra = "com `ChancePercent`% de chance, usar `Kind`" (com a `Skill` quando Kind = UseSkill).
// As regras são testadas na ordem; a primeira sorteada vence. Sem regras: só o ataque básico, e a
// IA NÃO consome aleatoriedade (o resultado dos combates atuais não muda).
public sealed record MonsterActionRule
{
    public MonsterActionRule(CombatActionKind kind, int chancePercent, SkillDefinition? skill = null)
    {
        if (chancePercent is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(chancePercent), "Chance must be between 1 and 100.");
        }

        if (kind == CombatActionKind.BasicAttack)
        {
            throw new ArgumentException("The basic attack is always executed; it is not a monster rule (ADR-006).", nameof(kind));
        }

        if (kind == CombatActionKind.UseSkill && skill is null)
        {
            throw new ArgumentException("A UseSkill rule needs the skill data.", nameof(skill));
        }

        Kind = kind;
        ChancePercent = chancePercent;
        Skill = skill;
    }

    public CombatActionKind Kind { get; }

    public int ChancePercent { get; }

    public SkillDefinition? Skill { get; }
}

public sealed record MonsterBehavior(IReadOnlyList<MonsterActionRule> Rules)
{
    // Comportamento atual de todos os monstros da Spider Cave (migrado para dados no PV-MONSTER-010).
    public static MonsterBehavior BasicAttackOnly { get; } = new([]);

    public IReadOnlyList<MonsterActionRule> Rules { get; } = (Rules ?? throw new ArgumentNullException(nameof(Rules))).ToList().AsReadOnly();
}
