using ProjectVictory.Domain.Entities;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-QA-001 parte 2: documenta o comportamento atual de Inventory, Skills e HuntSession.
public sealed class InventoryTests
{
    [Fact]
    public void NewInventory_IsEmpty()
    {
        var inventory = new Inventory();

        Assert.Equal(0, inventory.Count);
        Assert.Empty(inventory.Items);
    }

    [Fact]
    public void AddItem_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Inventory().AddItem(null!));
    }

    [Fact]
    public void Add_Contains_Get_Remove()
    {
        var inventory = new Inventory();
        var item = new ItemInstance(Guid.NewGuid(), Guid.NewGuid());

        inventory.AddItem(item);

        Assert.True(inventory.Contains(item.Id));
        Assert.Same(item, inventory.GetItem(item.Id));
        Assert.True(inventory.RemoveItem(item.Id));
        Assert.False(inventory.Contains(item.Id));
        Assert.False(inventory.RemoveItem(item.Id));
    }

    [Fact]
    public void Clear_RemovesAll()
    {
        var inventory = new Inventory();
        inventory.AddItem(new ItemInstance(Guid.NewGuid(), Guid.NewGuid()));
        inventory.AddItem(new ItemInstance(Guid.NewGuid(), Guid.NewGuid()));

        inventory.Clear();

        Assert.Equal(0, inventory.Count);
    }
}

public sealed class SkillsTests
{
    [Fact]
    public void NewSkills_StartAtTen()
    {
        var skills = new Skills();

        Assert.Equal(10, skills.Fist);
        Assert.Equal(10, skills.Sword);
        Assert.Equal(10, skills.MagicLevel);
    }

    [Fact]
    public void Increase_Positive_Adds()
    {
        var skills = new Skills();

        skills.IncreaseSword(3);

        Assert.Equal(13, skills.Sword);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Increase_NonPositive_Throws(int amount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Skills().IncreaseFist(amount));
    }
}

public sealed class HuntSessionTests
{
    [Fact]
    public void EmptyCharacter_Throws()
    {
        Assert.Throws<ArgumentException>(() => new HuntSession(Guid.NewGuid(), Guid.Empty));
    }

    [Fact]
    public void Lifecycle_Created_Running_Stopped()
    {
        var session = new HuntSession(Guid.NewGuid(), Guid.NewGuid());

        Assert.Equal(HuntSessionStatus.Created, session.Status);

        session.Start();
        Assert.Equal(HuntSessionStatus.Running, session.Status);
        Assert.NotNull(session.StartedAt);

        session.Stop();
        Assert.Equal(HuntSessionStatus.Stopped, session.Status);
        Assert.NotNull(session.StoppedAt);
    }

    [Fact]
    public void Stop_WithoutStart_Throws()
    {
        var session = new HuntSession(Guid.NewGuid(), Guid.NewGuid());

        Assert.Throws<InvalidOperationException>(() => session.Stop());
    }

    [Fact]
    public void Start_Twice_Throws()
    {
        var session = new HuntSession(Guid.NewGuid(), Guid.NewGuid());
        session.Start();

        Assert.Throws<InvalidOperationException>(() => session.Start());
    }
}
