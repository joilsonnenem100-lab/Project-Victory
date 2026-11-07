using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-COMBAT-014 (PO-53): 1 spell de cura por vocação; % da vida MÁXIMA por faixa de Level (dados).
public sealed class ClassHealingSpellTableTests
{
    [Fact]
    public void EveryClass_HasExactlyOneHealingSpell_ThatCostsMana()
    {
        foreach (var characterClass in Enum.GetValues<CharacterClass>())
        {
            var spells = ClassHealingSpellTable.All.Where(s => s.Class == characterClass).ToList();
            var spell = Assert.Single(spells);
            Assert.True(spell.ManaCost > 0);
            Assert.Same(spell, ClassHealingSpellTable.ForClass(characterClass));
        }
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(19, 10)]
    [InlineData(20, 15)]
    [InlineData(49, 15)]
    [InlineData(50, 20)]
    [InlineData(99, 20)]
    [InlineData(100, 25)]
    [InlineData(500, 25)]
    public void StandardBands_PercentImprovesWithLevelByBand(int level, int expectedPercent)
    {
        var knight = ClassHealingSpellTable.ForClass(CharacterClass.Knight)!;

        Assert.Equal(expectedPercent, knight.PercentForLevel(level));
    }

    [Fact]
    public void Druid_HealsMorePerBand_PO01()
    {
        var druid = ClassHealingSpellTable.ForClass(CharacterClass.Druid)!;
        var sorcerer = ClassHealingSpellTable.ForClass(CharacterClass.Sorcerer)!;

        foreach (var level in new[] { 1, 20, 50, 100 })
        {
            Assert.Equal(sorcerer.PercentForLevel(level) + 5, druid.PercentForLevel(level));
        }
    }

    [Fact]
    public void RawHealing_IsPercentOfMaximumHealth_FlooredWithMinimumOne()
    {
        var spell = new HealingSpellDefinition("t", "T", CharacterClass.Knight, 10, [new HealingBand(1, 10), new HealingBand(20, 15)]);

        Assert.Equal(15, spell.RawHealingFor(1, 155));   // 15,5 → 15
        Assert.Equal(30, spell.RawHealingFor(20, 200));
        Assert.Equal(1, spell.RawHealingFor(1, 5));      // 0,5 → mínimo 1
        Assert.Equal(0, new HealingSpellDefinition("z", "Z", CharacterClass.Knight, 10, [new HealingBand(10, 10)]).RawHealingFor(1, 100));
    }

    [Fact]
    public void Policy_WantsHealing_BelowThreshold_OnlyWhenEnabled()
    {
        Assert.Equal(50, HealingSpellPolicy.Default.BelowPercent);
        Assert.True(HealingSpellPolicy.Default.WantsHealing(49, 100));
        Assert.False(HealingSpellPolicy.Default.WantsHealing(50, 100));
        Assert.False(HealingSpellPolicy.Disabled.WantsHealing(1, 100));
        Assert.False(new HealingSpellPolicy(0).WantsHealing(0, 100));
    }
}
