using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Progression;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-PROG-013 (DEC-20261106-30): títulos e tier DERIVADOS do Level, tabela em configuração.
// Substitui os testes de Ascension da Fila 4 (PV-PROG-012), superados pelo DEC-20261106-30.
public sealed class AscensionTests
{
    [Theory]
    [InlineData(1, 0, "Novice")]
    [InlineData(499, 0, "Novice")]
    [InlineData(500, 1, "Ascension 1")]
    [InlineData(999, 1, "Ascension 1")]
    [InlineData(1000, 2, "Ascension 2")]
    [InlineData(1499, 2, "Ascension 2")]
    [InlineData(1500, 3, "Ascension 3")]
    [InlineData(1999, 3, "Ascension 3")]
    [InlineData(2000, 4, "Ascension 4")]
    [InlineData(2499, 4, "Ascension 4")]
    [InlineData(2500, 5, "Legend")]
    [InlineData(10_000, 5, "Legend")]
    [InlineData(821_009, 5, "Legend")]       // maior Level representável
    [InlineData(int.MaxValue, 5, "Legend")]  // Legend é permanente, sem teto
    public void DefaultTable_TitleAndTier_AtEveryLimit(int level, int tier, string label)
    {
        var title = LevelTitleTable.Default.For(level);

        Assert.Equal(tier, title.Tier);
        Assert.Equal(label, title.Label);
    }

    [Fact]
    public void Table_IsReplaceable_LabelsAndRanges()
    {
        var custom = new LevelTitleTable(
        [
            new LevelTitle(0, "Rookie", 1),
            new LevelTitle(1, "Veteran", 100),
            new LevelTitle(2, "Master", 200)
        ]);

        Assert.Equal("Rookie", custom.For(99).Label);
        Assert.Equal("Veteran", custom.For(100).Label);
        Assert.Equal(2, custom.For(5000).Tier);
        Assert.Equal(2, custom.MaximumTier);
    }

    [Fact]
    public void Table_RejectsInvalidConfiguration()
    {
        Assert.Throws<ArgumentException>(() => new LevelTitleTable([]));
        Assert.Throws<ArgumentException>(() => new LevelTitleTable([new LevelTitle(0, "A", 2)]));                         // não começa no 1
        Assert.Throws<ArgumentException>(() => new LevelTitleTable([new LevelTitle(0, "A", 1), new LevelTitle(2, "B", 5)])); // tier pulado
        Assert.Throws<ArgumentException>(() => new LevelTitleTable([new LevelTitle(0, "A", 1), new LevelTitle(1, "B", 1)])); // sobreposição
        Assert.Throws<ArgumentException>(() => new LevelTitleTable([new LevelTitle(0, " ", 1)]));                         // rótulo vazio
        Assert.Throws<ArgumentOutOfRangeException>(() => LevelTitleTable.Default.For(0));
    }

    [Fact]
    public void Character_TitleFollowsLevel_LevelNeverResets()
    {
        var definition = CharacterClassDefinition.Get(CharacterClass.Knight);
        var character = new ProjectVictory.Domain.Entities.Character(
            Guid.NewGuid(), new ProjectVictory.Domain.Account.AccountId(Guid.NewGuid()), "Asc", CharacterClass.Knight, definition,
            new ProjectVictory.Domain.ValueObjects.Health(150), new ProjectVictory.Domain.ValueObjects.Mana(55),
            new ProjectVictory.Domain.ValueObjects.Experience(ExperienceCurve.TotalExperienceForLevel(1000)));

        Assert.Equal(1000, character.Level.Value);
        Assert.Equal("Ascension 2", character.Title.Label);
        Assert.Equal(2, character.Title.Tier);
        Assert.Equal("Tier 1", character.TitleUnder(new LevelTitleTable([new LevelTitle(0, "Tier 0", 1), new LevelTitle(1, "Tier 1", 1000)])).Label);

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
