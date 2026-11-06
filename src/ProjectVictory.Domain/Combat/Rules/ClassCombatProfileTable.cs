using System;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Domain.Combat.Rules;

// PV-COMBAT-012: atributos de combate por classe, como dados.
// *** PROVISÓRIO *** — não há equipamento em uso nem tabela oficial de dano por classe.
// - AttackSkill: a skill da prioridade de dano da classe (knowledge-base/ascension.md: Knight → Sword;
//   Sorcerer → Magic Level). Paladin → Distance, Druid → Magic Level e Monk → Fist seguem a mesma lógica.
// - WeaponAttack, Armor e Defense representam "equipamento inicial" ainda inexistente.
// - Custo de mana e multiplicador da ClassAbility (a habilidade inicial da classe) também são
//   provisórios.
// Primeira calibração (2026-11-06): armadura/defesa iniciais altas tornavam o Knight imune aos
// Rats (dano máximo 4); reduzidas. Trocar valores = trocar só esta tabela.
public static class ClassCombatProfileTable
{
    private sealed record ClassBase(
        Func<Skills, int> AttackSkill,
        int WeaponAttack,
        int Armor,
        int Defense,
        int AbilityManaCost,
        decimal AbilityDamageMultiplier);

    private static ClassBase For(CharacterClass characterClass) => characterClass switch
    {
        CharacterClass.Knight => new(s => s.Sword, 14, 4, 6, 10, 1.5m),
        CharacterClass.Paladin => new(s => s.Distance, 15, 3, 4, 15, 1.5m),
        CharacterClass.Sorcerer => new(s => s.MagicLevel, 13, 2, 1, 20, 1.6m),
        CharacterClass.Druid => new(s => s.MagicLevel, 13, 2, 1, 20, 1.6m),
        CharacterClass.Monk => new(s => s.Fist, 12, 3, 3, 10, 1.5m),
        _ => throw new ArgumentOutOfRangeException(nameof(characterClass))
    };

    public static CombatProfile ForCharacter(
        CharacterClass characterClass,
        Skills skills)
    {
        ArgumentNullException.ThrowIfNull(skills);

        var classBase = For(characterClass);

        return new CombatProfile(
            classBase.AttackSkill(skills),
            classBase.WeaponAttack,
            classBase.Armor,
            classBase.Defense,
            classBase.AbilityManaCost,
            classBase.AbilityDamageMultiplier,
            // PV-COMBAT-013: força do ataque básico da classe (ClassSkillTable).
            ClassSkillTable.BasicAttackMultiplier(characterClass));
    }
}
