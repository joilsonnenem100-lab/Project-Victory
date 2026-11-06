using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;
using Xunit;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Domain.Tests;

// PV-CHAR-010: Vida e Mana máximas derivadas de Class + Level (ADR-029). Valores PROVISÓRIOS (R-021).
public sealed class DerivedAttributeTests
{
    private static CharacterEntity NewCharacter(CharacterClass characterClass, long experience)
    {
        var definition = CharacterClassDefinition.Get(characterClass);

        return new CharacterEntity(
            Guid.NewGuid(),
            new AccountId(Guid.NewGuid()),
            "Attr",
            characterClass,
            definition,
            new Health(definition.InitialHealth),
            new Mana(definition.InitialMana),
            new Experience(experience));
    }

    // PV-CHAR-011 (DEC-20261106-18, referência Tibia): Level 1 = 150/55 para todas as classes;
    // +5/+5 por Level até o 8 (185/90 no Level 8); depois, os ganhos da classe.
    // XP: Level 6 = 1.500; Level 8 = 4.200; Level 10 = 9.300 (curva do DEC-20261106-10).
    [Theory]
    [InlineData(CharacterClass.Knight, 15, 5)]
    [InlineData(CharacterClass.Paladin, 10, 15)]
    [InlineData(CharacterClass.Sorcerer, 5, 30)]
    [InlineData(CharacterClass.Druid, 5, 30)]
    [InlineData(CharacterClass.Monk, 12, 15)]
    public void Maximums_FollowTheTibiaCurve(CharacterClass characterClass, int hpPerLevel, int manaPerLevel)
    {
        var level1 = NewCharacter(characterClass, 0);
        var level6 = NewCharacter(characterClass, 1500);
        var level8 = NewCharacter(characterClass, 4200);
        var level10 = NewCharacter(characterClass, 9300);

        Assert.Equal((150, 55), (level1.MaximumHealth.Value, level1.MaximumMana.Value));
        Assert.Equal((175, 80), (level6.MaximumHealth.Value, level6.MaximumMana.Value));
        Assert.Equal(8, level8.Level.Value);
        Assert.Equal((185, 90), (level8.MaximumHealth.Value, level8.MaximumMana.Value));
        Assert.Equal(10, level10.Level.Value);
        Assert.Equal(185 + (2 * hpPerLevel), level10.MaximumHealth.Value);
        Assert.Equal(90 + (2 * manaPerLevel), level10.MaximumMana.Value);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(100)]
    [InlineData(1000)]
    public void Knight_MatchesTibiaFormula(int level)
    {
        // Tibia (Knight, Level ≥ 8): vida = 15 × Level + 65; mana = 5 × Level + 50.
        var curve = AttributeCurveTable.ForClass(CharacterClass.Knight);

        Assert.Equal((15 * level) + 65, curve.MaximumHealthAt(level));
        Assert.Equal((5 * level) + 50, curve.MaximumManaAt(level));
    }

    [Fact]
    public void Maximums_GrowWhenExperienceIsGained()
    {
        var knight = NewCharacter(CharacterClass.Knight, 0);

        knight.GainExperience(new Experience(100));

        Assert.Equal(2, knight.Level.Value);
        Assert.Equal(155, knight.MaximumHealth.Value);
        Assert.Equal(60, knight.MaximumMana.Value);
    }

    [Fact]
    public void Curve_SupportsFractionalGrowth_Floored()
    {
        // Formato de knowledge-base/progression.md (ex.: +0.75 por Level), caso o PO o confirme.
        var curve = new AttributeCurve(100, 0.75m, 50, 0.25m);

        Assert.Equal(100, curve.MaximumHealthAt(1));
        Assert.Equal(100, curve.MaximumHealthAt(2));
        Assert.Equal(101, curve.MaximumHealthAt(3));
        Assert.Equal(174, curve.MaximumHealthAt(100));
        Assert.Equal(74, curve.MaximumManaAt(100));
        Assert.Throws<ArgumentOutOfRangeException>(() => curve.MaximumHealthAt(0));
    }

    [Fact]
    public void LegacyCharacterWithoutClassDefinition_KeepsFixedMaximum()
    {
        var legacy = new CharacterEntity(
            Guid.NewGuid(),
            new AccountId(Guid.NewGuid()),
            "Legacy",
            new Health(90),
            new Mana(40),
            new Experience(5000));

        Assert.Equal(90, legacy.MaximumHealth.Value);
        Assert.Equal(40, legacy.MaximumMana.Value);
    }
}
