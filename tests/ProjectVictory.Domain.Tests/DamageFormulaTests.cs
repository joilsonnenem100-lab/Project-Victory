using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Entities;
using ProjectVictory.Domain.Primitives;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-COMBAT-012: fórmula de dano PROVISÓRIA inspirada no Tibia, atrás de IDamageFormula.
public sealed class DamageFormulaTests
{
    private sealed class FixedRandom(params int[] picks) : IRandomSource
    {
        private int _index;

        public double NextDouble() => 0;

        public int NextInclusive(int minInclusive, int maxInclusive) =>
            Math.Clamp(picks[_index++ % picks.Length], minInclusive, maxInclusive);
    }

    private static Combatant Fighter(int level, CombatProfile profile, CombatSide side = CombatSide.Player) =>
        new(Guid.NewGuid(), Guid.NewGuid(), side, "F", level, 100, 100, 50, 50, profile);

    [Theory]
    [InlineData(1, 10, 14, 12)]   // 0,085×10×14 + 1/5 = 12,1 → 12
    [InlineData(50, 10, 14, 21)]  // 11,9 + 10 = 21,9 → 21
    [InlineData(1, 8, 6, 4)]      // Rat: 4,08 + 0,2 → 4
    public void MaximumDamage_FollowsLevelSkillAndWeapon(int level, int skill, int weapon, int expected)
    {
        var formula = new ProvisionalTibiaLikeDamageFormula();

        Assert.Equal(expected, formula.MaximumDamage(Fighter(level, new CombatProfile(skill, weapon, 0, 0)), CombatActionKind.BasicAttack));
    }

    [Fact]
    public void ClassAbility_UsesMultiplier()
    {
        var formula = new ProvisionalTibiaLikeDamageFormula();
        var knight = Fighter(1, new CombatProfile(10, 14, 0, 0, 10, 1.5m));

        Assert.Equal(18, formula.MaximumDamage(knight, CombatActionKind.ClassAbility)); // 12,1 × 1,5 = 18,15
    }

    [Fact]
    public void Armor_AndDefense_ReduceDamage_NeverNegative()
    {
        var formula = new ProvisionalTibiaLikeDamageFormula();
        var attacker = Fighter(1, new CombatProfile(10, 14, 0, 0));
        var armored = Fighter(1, new CombatProfile(1, 1, 8, 10), CombatSide.Monster);

        // bruto 12; armadura sorteia em [4, 7] → 4; defesa em [0, 5] → 3 ⇒ 12 − 4 − 3 = 5.
        Assert.Equal(5, formula.RollDamage(attacker, armored, CombatActionKind.BasicAttack, new FixedRandom(12, 4, 3)));

        // bruto 2 contra reduções 7 + 5 ⇒ 0, nunca negativo.
        Assert.Equal(0, formula.RollDamage(attacker, armored, CombatActionKind.BasicAttack, new FixedRandom(2, 7, 5)));
    }

    [Fact]
    public void SameSeed_SameDamageSequence()
    {
        var formula = new ProvisionalTibiaLikeDamageFormula();
        var attacker = Fighter(10, new CombatProfile(20, 14, 0, 0));
        var defender = Fighter(5, new CombatProfile(5, 5, 4, 4), CombatSide.Monster);
        var a = new SeededRandomSource(77);
        var b = new SeededRandomSource(77);

        for (var i = 0; i < 30; i++)
        {
            Assert.Equal(
                formula.RollDamage(attacker, defender, CombatActionKind.BasicAttack, a),
                formula.RollDamage(attacker, defender, CombatActionKind.BasicAttack, b));
        }
    }

    [Theory]
    [InlineData(CharacterClass.Knight, 14, 4, 6, 10)]
    [InlineData(CharacterClass.Paladin, 15, 3, 4, 15)]
    [InlineData(CharacterClass.Sorcerer, 13, 2, 1, 20)]
    [InlineData(CharacterClass.Druid, 13, 2, 1, 20)]
    [InlineData(CharacterClass.Monk, 12, 3, 3, 10)]
    public void ClassProfiles_AreData(CharacterClass characterClass, int weapon, int armor, int defense, int abilityCost)
    {
        var profile = ClassCombatProfileTable.ForCharacter(characterClass, new Skills());

        Assert.Equal(10, profile.AttackSkill); // todas as skills começam em 10
        Assert.Equal(weapon, profile.WeaponAttack);
        Assert.Equal(armor, profile.Armor);
        Assert.Equal(defense, profile.Defense);
        Assert.Equal(abilityCost, profile.AbilityManaCost);
    }
}
