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

        // Revisão do PV-MONSTER-010 (ressalva 1): a regra de monstro é uma HABILIDADE da espécie
        // (ADR-005). Basic Attack é sempre executado (ADR-006); poções e a habilidade de classe são do
        // jogador e não podem ser regras de monstro.
        if (kind != CombatActionKind.UseSkill)
        {
            throw new ArgumentException("A monster rule must be a UseSkill ability (ADR-005/006).", nameof(kind));
        }

        if (skill is null)
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
