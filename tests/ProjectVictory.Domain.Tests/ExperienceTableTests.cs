using ProjectVictory.Domain.Progression;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-PROG-011: Experience Table gerada pela curva do DEC-20261106-10 (ADR-025, ADR-026).
public sealed class ExperienceTableTests
{
    [Theory]
    [InlineData(1, 0L)]
    [InlineData(2, 100L)]
    [InlineData(3, 200L)]
    [InlineData(4, 400L)]
    [InlineData(5, 800L)]
    [InlineData(6, 1500L)]
    [InlineData(500, 2058474800L)]
    public void Curve_MatchesFormula(int level, long expected)
    {
        Assert.Equal(expected, ExperienceCurve.TotalExperienceForLevel(level));
    }

    [Fact]
    public void Curve_IsExactIntegerDivision_ForAllLevelsOfTheTable()
    {
        for (long l = 1; l <= ExperienceTable.MaximumLevelPerAscension; l++)
        {
            var polynomial = (l * l * l) - (6 * l * l) + (17 * l) - 12;
            Assert.Equal(0, (50 * polynomial) % 3);
        }
    }

    [Fact]
    public void Curve_LevelBelowOne_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceCurve.TotalExperienceForLevel(0));
    }

    // PV-PROG-012: substitui "Standard tem 500 entradas" — a tabela padrão é a curva, sem teto.
    [Fact]
    public void Standard_IsTheUnboundedCurve_ExplicitTablesStillAvailable()
    {
        var table = ExperienceTable.Standard;

        Assert.True(table.IsUnbounded);
        Assert.Empty(table.Entries);
        Assert.Equal(ExperienceCurve.MaximumSupportedLevel, table.MaximumLevel);
        Assert.Equal(569_256, ExperienceCurve.MaximumSupportedLevel);

        var explicit500 = ExperienceTable.FromCurve(500);
        Assert.Equal(500, explicit500.Entries.Count);
        Assert.Equal(500, explicit500.Entries[^1].Level);
    }

    [Theory]
    [InlineData(0L, 1)]
    [InlineData(99L, 1)]
    [InlineData(100L, 2)]
    [InlineData(199L, 2)]
    [InlineData(200L, 3)]
    [InlineData(399L, 3)]
    [InlineData(400L, 4)]
    [InlineData(1000L, 5)]
    [InlineData(1500L, 6)]
    public void GetLevel_InterpretsExperience(long experience, int expectedLevel)
    {
        Assert.Equal(expectedLevel, ExperienceTable.Standard.GetLevel(experience));
    }

    // PV-PROG-012: substitui "nunca passa do Level 500" — o Level continua pela curva do Tibia.
    [Fact]
    public void GetLevel_HasNoCap_FollowsTheCurveAfter500()
    {
        Assert.Equal(500, ExperienceTable.Standard.GetLevel(2_058_474_800L));
        Assert.Equal(500, ExperienceTable.Standard.GetLevel(2_070_899_999L));
        Assert.Equal(501, ExperienceTable.Standard.GetLevel(2_070_900_000L));
        Assert.Equal(566, ExperienceTable.Standard.GetLevel(3_000_000_000L));
        Assert.Equal(1000, ExperienceTable.Standard.GetLevel(ExperienceCurve.TotalExperienceForLevel(1000)));
        Assert.Equal(ExperienceCurve.MaximumSupportedLevel, ExperienceTable.Standard.GetLevel(long.MaxValue));
    }

    [Fact]
    public void Curve_OverflowIsDetected_NeverWrong()
    {
        Assert.Throws<OverflowException>(() => ExperienceCurve.TotalExperienceForLevel(ExperienceCurve.MaximumSupportedLevel + 1));
        Assert.True(ExperienceCurve.TotalExperienceForLevel(ExperienceCurve.MaximumSupportedLevel) > 0);
    }

    [Fact]
    public void GetLevel_NegativeExperience_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceTable.Standard.GetLevel(-1));
    }

    [Fact]
    public void GetExperienceToNextLevel_ReturnsRemaining_AndNullOnlyAtLastRepresentableLevel()
    {
        Assert.Equal(100, ExperienceTable.Standard.GetExperienceToNextLevel(0));
        Assert.Equal(50, ExperienceTable.Standard.GetExperienceToNextLevel(150));
        Assert.Equal(6_082_600, ExperienceTable.Standard.GetExperienceToNextLevel(3_000_000_000L)); // Level 566 → 567
        Assert.Null(ExperienceTable.Standard.GetExperienceToNextLevel(long.MaxValue));
    }

    [Fact]
    public void ForAscension_SameCurveForEveryAscension()
    {
        // DEC-20261106-11.
        Assert.Same(ExperienceTable.Standard, ExperienceTable.ForAscension(1));
        Assert.Same(ExperienceTable.Standard, ExperienceTable.ForAscension(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceTable.ForAscension(0));
    }

    [Fact]
    public void Validation_RejectsGapsOverlapsAndWrongStart()
    {
        Assert.Throws<ArgumentException>(() => new ExperienceTable(Array.Empty<ExperienceTableEntry>()));
        Assert.Throws<ArgumentException>(() => new ExperienceTable(new[]
        {
            new ExperienceTableEntry(1, 10)
        }));
        Assert.Throws<ArgumentException>(() => new ExperienceTable(new[]
        {
            new ExperienceTableEntry(1, 0),
            new ExperienceTableEntry(3, 100)
        }));
        Assert.Throws<ArgumentException>(() => new ExperienceTable(new[]
        {
            new ExperienceTableEntry(1, 0),
            new ExperienceTableEntry(2, 0)
        }));
    }

    [Fact]
    public void LegacyDictionaryConstructor_StillWorks()
    {
        var table = new ExperienceTable(new Dictionary<int, int> { { 1, 0 }, { 2, 50 } });

        Assert.Equal(50, table.GetRequiredExperience(2));
        Assert.Equal(0, table.GetRequiredExperience(3));
        Assert.Equal(2, table.GetLevel(60));
    }

    [Fact]
    public void LevelCalculator_UsesTheTable()
    {
        Assert.Equal(3, new LevelCalculator().Calculate(new Experience(250)).Value);
    }
}

// Ressalvas da revisão do PV-PROG-011.
public sealed class ExperienceTableReviewTests
{
    [Fact]
    public void MaximumExperience_IsDerived_AndOpenOnlyAtLastRepresentableLevel()
    {
        Assert.Equal(99, ExperienceTable.Standard.GetMaximumExperience(1));
        Assert.Equal(199, ExperienceTable.Standard.GetMaximumExperience(2));
        Assert.Equal(2_070_899_999L, ExperienceTable.Standard.GetMaximumExperience(500)); // não é mais o último
        Assert.Null(ExperienceTable.Standard.GetMaximumExperience(ExperienceCurve.MaximumSupportedLevel));
    }

    // PV-PROG-012: substitui "tabela não passa de 500" — o limite de 500 entradas foi removido.
    [Fact]
    public void ExplicitTable_CanExceed500_ButNotTheSupportedLevel()
    {
        Assert.Equal(501, ExperienceTable.FromCurve(501).MaximumLevel);
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceTable.FromCurve(ExperienceCurve.MaximumSupportedLevel + 1));
    }

    [Fact]
    public void GetRequiredExperience_ThrowsOverflow_WhenMinimumExceedsInt()
    {
        var table = new ExperienceTable(new[]
        {
            new ExperienceTableEntry(1, 0),
            new ExperienceTableEntry(2, 3_000_000_000L)
        });

        Assert.Throws<OverflowException>(() => table.GetRequiredExperience(2));
    }

    [Fact]
    public void Entries_CannotBeMutatedByCast()
    {
        var entries = ExperienceTable.FromCurve(500).Entries;

        Assert.False(entries is ExperienceTableEntry[]);
    }
}
