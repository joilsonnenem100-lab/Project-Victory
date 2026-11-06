using ProjectVictory.Application.Reward;
using Xunit;

namespace ProjectVictory.Application.Tests;

// PV-XP-010 (ADR-007): Experience Distribution reparte só o valor FINAL; não calcula nem aplica bônus.
public sealed class ExperienceDistributionTests
{
    private readonly EvenSplitExperienceDistributor _distributor = EvenSplitExperienceDistributor.Instance;

    [Theory]
    [InlineData(10L)]
    [InlineData(0L)]
    [InlineData(3_000_000_000L)]
    public void SingleParticipant_GetsTheWholeExperience(long experience)
    {
        var characterId = Guid.NewGuid();

        var share = Assert.Single(_distributor.Distribute(experience, [characterId]));

        Assert.Equal(characterId, share.CharacterId);
        Assert.Equal(experience, share.Amount);
    }

    [Fact]
    public void SeveralParticipants_EvenSplit_RemainderToTheFirst_SumIsPreserved()
    {
        Guid[] party = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        var shares = _distributor.Distribute(100, party);

        Assert.Equal(new long[] { 34, 33, 33 }, shares.Select(s => s.Amount));
        Assert.Equal(party, shares.Select(s => s.CharacterId));
        Assert.Equal(100, shares.Sum(s => s.Amount));
    }

    [Fact]
    public void InvalidInput_IsRejected()
    {
        var id = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => _distributor.Distribute(10, []));
        Assert.Throws<ArgumentException>(() => _distributor.Distribute(10, [id, id]));
        Assert.Throws<ArgumentOutOfRangeException>(() => _distributor.Distribute(-1, [id]));
    }
}
