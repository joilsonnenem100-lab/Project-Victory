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

    [Fact]
    public void Standard_HasExplicitEntriesForLevels1To500()
    {
        var table = ExperienceTable.Standard;

        Assert.Equal(500, table.MaximumLevel);
        Assert.Equal(500, table.Entries.Count);
        Assert.Equal(1, table.Entries[0].Level);
        Assert.Equal(0, table.Entries[0].MinimumExperience);
        Assert.Equal(500, table.Entries[^1].Level);
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

    [Fact]
    public void GetLevel_NeverPassesLevel500_ExperienceKeepsCounting()
    {
        // ADR-026: limite de Level 500 por Ascension; ADR-023: a Experience continua absoluta.
        Assert.Equal(500, ExperienceTable.Standard.GetLevel(2058474800L));
        Assert.Equal(500, ExperienceTable.Standard.GetLevel(long.MaxValue));
        Assert.Equal(499, ExperienceTable.Standard.GetLevel(2058474799L));
    }

    [Fact]
    public void GetLevel_NegativeExperience_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceTable.Standard.GetLevel(-1));
    }

    [Fact]
    public void GetExperienceToNextLevel_ReturnsRemaining_AndNullAtLastLevel()
    {
        Assert.Equal(100, ExperienceTable.Standard.GetExperienceToNextLevel(0));
        Assert.Equal(50, ExperienceTable.Standard.GetExperienceToNextLevel(150));
        Assert.Null(ExperienceTable.Standard.GetExperienceToNextLevel(3000000000L));
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
    public void MaximumExperience_IsDerived_AndOpenAtLastLevel()
    {
        Assert.Equal(99, ExperienceTable.Standard.GetMaximumExperience(1));
        Assert.Equal(199, ExperienceTable.Standard.GetMaximumExperience(2));
        Assert.Null(ExperienceTable.Standard.GetMaximumExperience(500));
    }

    [Fact]
    public void Table_CannotExceedAscensionLimit()
    {
        Assert.Throws<ArgumentException>(() => ExperienceTable.FromCurve(501));
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
        var entries = ExperienceTable.Standard.Entries;

        Assert.False(entries is ExperienceTableEntry[]);
    }
}
