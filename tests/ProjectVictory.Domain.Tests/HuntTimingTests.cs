using ProjectVictory.Domain.Hunt;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-HUNT-011 (revisão, ressalva 5d): medição da Hunt no Domain.
public sealed class HuntTimingTests
{
    private static readonly DateTimeOffset T0 = new(2026, 11, 6, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Timing_StartEndAndExperience()
    {
        var hunt = new ProjectVictory.Domain.Hunt.Hunt(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        Assert.Equal(TimeSpan.Zero, hunt.ElapsedAt(T0));            // sem início: zero

        hunt.Start(T0);
        Assert.Equal(TimeSpan.FromMinutes(5), hunt.ElapsedAt(T0.AddMinutes(5)));

        Assert.Throws<InvalidOperationException>(() => hunt.MarkEnded(T0.AddMinutes(5))); // ainda rodando
        Assert.Throws<ArgumentOutOfRangeException>(() => hunt.RecordExperience(-1));

        hunt.RecordExperience(30);
        hunt.RecordExperience(12);
        hunt.Complete();
        hunt.MarkEnded(T0.AddMinutes(10));
        hunt.MarkEnded(T0.AddMinutes(99));                           // só a primeira vale

        Assert.Equal(42, hunt.ExperienceGained);
        Assert.Equal(TimeSpan.FromMinutes(10), hunt.ElapsedAt(T0.AddHours(3)));
    }
}
