using ProjectVictory.Domain.Spawn;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-SPAWN-010 (ADR-047): máquina de estados explícita da Spawn Instance.
public sealed class SpawnInstanceTests
{
    private static readonly DateTime Now = new(2026, 11, 6, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Lifecycle_FollowsTheExplicitStateMachine()
    {
        var instance = new SpawnInstance(Guid.NewGuid(), Guid.NewGuid());
        Assert.Equal(SpawnState.Created, instance.CurrentState);

        instance.BeginSpawning();
        Assert.Equal(SpawnState.Spawning, instance.CurrentState);

        instance.Activate(Now);
        Assert.Equal(SpawnState.Active, instance.CurrentState);
        Assert.Equal(Now, instance.SpawnedAt);

        instance.BeginDespawning();
        Assert.Equal(SpawnState.Despawning, instance.CurrentState);

        instance.Despawn(Now.AddMinutes(1));
        Assert.Equal(SpawnState.Despawned, instance.CurrentState);
        Assert.Equal(Now.AddMinutes(1), instance.DespawnedAt);
    }

    [Fact]
    public void OutOfOrderTransitions_AreRejected()
    {
        var instance = new SpawnInstance(Guid.NewGuid(), Guid.NewGuid());

        Assert.Throws<InvalidOperationException>(() => instance.Activate(Now));       // Created → Active
        Assert.Throws<InvalidOperationException>(() => instance.BeginDespawning());   // Created → Despawning

        instance.BeginSpawning();
        instance.Activate(Now);
        Assert.Throws<InvalidOperationException>(() => instance.Despawn(Now));        // Active → Despawned
        Assert.Throws<InvalidOperationException>(() => instance.BeginSpawning());     // nunca volta

        Assert.Throws<ArgumentException>(() => new SpawnInstance(Guid.Empty, Guid.NewGuid()));
        Assert.Throws<ArgumentException>(() => new SpawnInstance(Guid.NewGuid(), Guid.Empty));
    }
}
