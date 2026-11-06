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

    [Theory]
    [InlineData(CharacterClass.Knight, 185, 55, 15, 5)]
    [InlineData(CharacterClass.Paladin, 155, 95, 10, 15)]
    [InlineData(CharacterClass.Sorcerer, 125, 140, 5, 30)]
    [InlineData(CharacterClass.Druid, 125, 140, 5, 30)]
    [InlineData(CharacterClass.Monk, 150, 80, 10, 10)]
    public void Maximums_FollowTheCurve(CharacterClass characterClass, int hp1, int mana1, int hpPerLevel, int manaPerLevel)
    {
        // Level 1 (0 XP) = valores atuais de criação; Level 6 (1500 XP) = +5 Levels.
        var level1 = NewCharacter(characterClass, 0);
        var level6 = NewCharacter(characterClass, 1500);

        Assert.Equal(hp1, level1.MaximumHealth.Value);
        Assert.Equal(mana1, level1.MaximumMana.Value);
        Assert.Equal(6, level6.Level.Value);
        Assert.Equal(hp1 + (5 * hpPerLevel), level6.MaximumHealth.Value);
        Assert.Equal(mana1 + (5 * manaPerLevel), level6.MaximumMana.Value);
    }

    [Fact]
    public void Maximums_GrowWhenExperienceIsGained()
    {
        var knight = NewCharacter(CharacterClass.Knight, 0);

        knight.GainExperience(new Experience(100));

        Assert.Equal(2, knight.Level.Value);
        Assert.Equal(200, knight.MaximumHealth.Value);
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
