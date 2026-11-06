using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.ValueObjects;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-QA-001 parte 2: documenta o comportamento atual dos value objects existentes.
public sealed class ValueObjectTests
{
    [Fact]
    public void Experience_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Experience(-1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(150)]
    public void Experience_NonNegative_KeepsValue(int value)
    {
        Assert.Equal(value, new Experience(value).Value);
    }

    [Fact]
    public void Experience_SameValue_AreEqual()
    {
        Assert.Equal(new Experience(10), new Experience(10));
        Assert.NotEqual(new Experience(10), new Experience(11));
    }

    [Fact]
    public void Level_BelowOne_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Level(0));
    }

    [Fact]
    public void Level_One_IsValid()
    {
        Assert.Equal(1, new Level(1).Value);
    }

    [Fact]
    public void Health_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Health(-5));
    }

    [Fact]
    public void Mana_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Mana(-5));
    }

    [Fact]
    public void AccountId_Empty_Throws()
    {
        Assert.Throws<ArgumentException>(() => new AccountId(Guid.Empty));
    }

    [Fact]
    public void AccountId_SameGuid_AreEqual()
    {
        var id = Guid.NewGuid();

        Assert.Equal(new AccountId(id), new AccountId(id));
        Assert.NotEqual(AccountId.New(), AccountId.New());
    }
}
