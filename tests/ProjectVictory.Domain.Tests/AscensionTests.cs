using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Progression;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-PROG-012 (DEC-20261106-19): Ascension derivada do Level; passo e máximo em configuração.
public sealed class AscensionTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(499, 1)]
    [InlineData(500, 2)]
    [InlineData(999, 2)]
    [InlineData(1000, 3)]
    [InlineData(1499, 3)]
    [InlineData(1500, 4)]
    [InlineData(2000, 4)]     // máximo atual 4
    [InlineData(500_000, 4)]
    public void DefaultRules_NewAscensionEvery500Levels_UpToFour(int level, int expected)
    {
        Assert.Equal(expected, AscensionRules.Default.AscensionFor(level));
    }

    [Fact]
    public void Rules_AreConfigurable_ForFutureExpansion()
    {
        var expanded = new AscensionRules(LevelStep: 500, MaximumAscension: 6);

        Assert.Equal(5, expanded.AscensionFor(2000));
        Assert.Equal(6, expanded.AscensionFor(9999));
        Assert.Equal(2500, expanded.UnlockLevel(6));
        Assert.Equal(1, expanded.UnlockLevel(1));
        Assert.Equal(3, new AscensionRules(LevelStep: 100, MaximumAscension: 4).AscensionFor(250));
    }

    [Fact]
    public void Rules_RejectInvalidValues()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AscensionRules(LevelStep: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new AscensionRules(MaximumAscension: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => AscensionRules.Default.AscensionFor(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => AscensionRules.Default.UnlockLevel(5));
    }

    [Fact]
    public void Character_AscensionFollowsLevel_LevelNeverResets()
    {
        var definition = CharacterClassDefinition.Get(CharacterClass.Knight);
        var character = new ProjectVictory.Domain.Entities.Character(
            Guid.NewGuid(), new ProjectVictory.Domain.Account.AccountId(Guid.NewGuid()), "Asc", CharacterClass.Knight, definition,
            new ProjectVictory.Domain.ValueObjects.Health(150), new ProjectVictory.Domain.ValueObjects.Mana(55),
            new ProjectVictory.Domain.ValueObjects.Experience(ExperienceCurve.TotalExperienceForLevel(1000)));

        Assert.Equal(1000, character.Level.Value);
        Assert.Equal(3, character.Ascension);
        Assert.Equal(2, character.AscensionUnder(new AscensionRules(LevelStep: 1000, MaximumAscension: 4)));

        // Vida máxima continua crescendo com o Level (sem voltar a 1): Knight 15 × L + 65.
        Assert.Equal((15 * 1000) + 65, character.MaximumHealth.Value);
    }

    // --- ressalvas da revisão do PV-CHAR-011 ---

    [Theory]
    [InlineData(CharacterClass.Paladin, 100, 185 + (10 * 92), 90 + (15 * 92))]
    [InlineData(CharacterClass.Druid, 100, (5 * 100) + 145, 90 + (30 * 92))]
    [InlineData(CharacterClass.Sorcerer, 1000, (5 * 1000) + 145, 90 + (30 * 992))]
    public void OtherClasses_MatchTibiaFormulaAtHighLevel(CharacterClass characterClass, int level, int health, int mana)
    {
        var curve = AttributeCurveTable.ForClass(characterClass);

        Assert.Equal(health, curve.MaximumHealthAt(level));
        Assert.Equal(mana, curve.MaximumManaAt(level));
    }

    [Fact]
    public void AttributeCurve_OverflowAndInvalidVocationLevel_AreErrors()
    {
        Assert.Throws<OverflowException>(() => new AttributeCurve(150, 1_000_000m, 55, 5m, 8, 5m, 5m).MaximumHealthAt(500_000));
        Assert.Throws<InvalidOperationException>(() => new AttributeCurve(150, 15m, 55, 5m, 0).MaximumHealthAt(10));
    }

    [Fact]
    public void Combatant_ClampsStoredCurrentHealthToTheNewMaximum()
    {
        // Personagem antigo com 185 de vida atual e máximo novo 150 (PV-CHAR-011): o combate usa 150.
        var player = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "Old", 1, 150, 185, 55, 90, new CombatProfile(10, 14, 4, 6));

        Assert.Equal(150, player.CurrentHealth);
        Assert.Equal(55, player.CurrentMana);
    }
}
