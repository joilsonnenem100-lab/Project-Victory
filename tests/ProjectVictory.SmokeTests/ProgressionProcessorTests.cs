using ProjectVictory.Application.Progression;
using ProjectVictory.Infrastructure.Services;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-PROG-011: o ProgressionProcessor interpreta a Experience Table (sem teto fixo em 5).
public sealed class ProgressionProcessorTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(100, 2)]
    [InlineData(200, 3)]
    [InlineData(1500, 6)]
    [InlineData(100000, 20)]
    public void Process_ResolvesLevelFromTable(int experience, int expectedLevel)
    {
        var result = new ProgressionProcessor().Process(
            new ProgressionRequest(Guid.NewGuid(), experience));

        Assert.Equal(experience, result.Experience);
        Assert.Equal(expectedLevel, result.Level);
    }
}
