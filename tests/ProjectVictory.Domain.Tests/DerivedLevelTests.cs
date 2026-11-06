using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;
using Xunit;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Domain.Tests;

// PV-PROG-010: Level derivado de Experience + Ascension + Experience Table (ADR-027, ADR-028).
public sealed class DerivedLevelTests
{
    private static CharacterEntity NewCharacter(long experience = 0)
    {
        var definition = CharacterClassDefinition.Get(CharacterClass.Knight);

        return new CharacterEntity(
            Guid.NewGuid(),
            new AccountId(Guid.NewGuid()),
            "Tester",
            CharacterClass.Knight,
            definition,
            new Health(definition.InitialHealth),
            new Mana(definition.InitialMana),
            new Experience(experience));
    }

    [Fact]
    public void NewCharacter_IsLevel1_InFirstAscension()
    {
        var character = NewCharacter();

        Assert.Equal(1, character.Level.Value);
        Assert.Equal(0, character.Title.Tier); // PV-PROG-013: Level 1 = Novice (tier 0)
    }

    [Theory]
    [InlineData(99L, 1)]
    [InlineData(100L, 2)]
    [InlineData(1500L, 6)]
    public void Level_FollowsExperience(long experience, int expectedLevel)
    {
        Assert.Equal(expectedLevel, NewCharacter(experience).Level.Value);
    }

    [Fact]
    public void GainExperience_ChangesDerivedLevel()
    {
        var character = NewCharacter();

        character.GainExperience(new Experience(800));

        Assert.Equal(800, character.Experience.Value);
        Assert.Equal(5, character.Level.Value);
    }

    // PV-PROG-012 (DEC-20261106-19): substitui "Level para no 500" — o Level não tem teto.
    [Fact]
    public void Experience_AboveIntRange_IsSupported_AndLevelHasNoCap()
    {
        var character = NewCharacter(int.MaxValue);

        character.GainExperience(new Experience(1_000_000_000L));

        Assert.Equal(3_147_483_647L, character.Experience.Value);
        Assert.Equal(575, character.Level.Value);
        Assert.Equal("Ascension 1", character.Title.Label); // PV-PROG-013: Level 500–999 → Ascension 1 (tier 1)
    }

    [Fact]
    public void ApplyLevel_CannotSetLevelDifferentFromDerived()
    {
        var character = NewCharacter(100);

        Assert.Throws<InvalidOperationException>(() => character.ApplyLevel(7));

        character.ApplyLevel(2);
        Assert.Equal(2, character.Level.Value);
    }
}
