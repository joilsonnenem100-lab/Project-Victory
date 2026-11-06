using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-MONSTER-010 (revisão, ressalvas 1 e 4): validação dos dados de comportamento de monstro.
public sealed class MonsterBehaviorTests
{
    private static readonly SkillDefinition Bite =
        new("monster-bite", "Bite", CharacterClass.Knight, SkillTargeting.SingleTarget, 0, 2m, 1, SkillScaling.Weapon);

    [Fact]
    public void Rules_AreValidated_OnlyMonsterAbilities()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonsterActionRule(CombatActionKind.UseSkill, 0, Bite));
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonsterActionRule(CombatActionKind.UseSkill, 101, Bite));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.UseSkill, 50));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.BasicAttack, 50, Bite));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.UseHealthPotion, 50));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.UseManaPotion, 50));
        Assert.Throws<ArgumentException>(() => new MonsterActionRule(CombatActionKind.ClassAbility, 50, Bite));

        var rule = new MonsterActionRule(CombatActionKind.UseSkill, 100, Bite);
        Assert.Equal(100, rule.ChancePercent);
        Assert.Empty(MonsterBehavior.BasicAttackOnly.Rules);
    }
}
