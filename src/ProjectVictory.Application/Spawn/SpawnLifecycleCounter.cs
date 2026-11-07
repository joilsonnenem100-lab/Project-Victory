using ProjectVictory.Application.Events;
using ProjectVictory.Domain.Events;

namespace ProjectVictory.Application.Spawn;

// PV-EVENT-011 (R-046): observa o ciclo de vida do Spawn pelos Domain Events (contadores; memória fixa).
public sealed class SpawnLifecycleCounter :
    IEventHandler<SpawnInstanceActivatedEvent>,
    IEventHandler<SpawnInstanceDespawnedEvent>
{
    private long _activated;
    private long _despawned;

    public long Activated => Interlocked.Read(ref _activated);

    public long Despawned => Interlocked.Read(ref _despawned);

    public void Handle(SpawnInstanceActivatedEvent domainEvent) => Interlocked.Increment(ref _activated);

    public void Handle(SpawnInstanceDespawnedEvent domainEvent) => Interlocked.Increment(ref _despawned);
}
