using System.Collections.Concurrent;
using ProjectVictory.Domain.Spawn;

namespace ProjectVictory.Application.Hunt;

// PV-HUNT-011 (checklist do PV-STATE-001 D3): serializa os comandos sobre a MESMA Hunt (ataque,
// saída manual, queda) — um por vez; Hunts diferentes seguem em paralelo.
public interface IHuntLock
{
    Task<IDisposable> AcquireAsync(Guid huntId, CancellationToken cancellationToken = default);

    // Descarta o lock de uma Hunt encerrada (memória).
    void Release(Guid huntId);
}

public sealed class HuntLock : IHuntLock
{
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

    public async Task<IDisposable> AcquireAsync(Guid huntId, CancellationToken cancellationToken = default)
    {
        var semaphore = _locks.GetOrAdd(huntId, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(cancellationToken);
        return new Releaser(semaphore);
    }

    public void Release(Guid huntId) => _locks.TryRemove(huntId, out _);

    public int Count => _locks.Count;

    private sealed class Releaser(SemaphoreSlim semaphore) : IDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                semaphore.Release();
            }
        }
    }
}

// PV-HUNT-011 (fecha o R-044): Spawn Instances ATIVAS por encounter (estado Temporário, em memória),
// para o despawn quando o encounter termina ou a Hunt acaba (ADR-009/047). O Spawn System continua
// stateless: quem guarda as instâncias em uso é este registro.
public interface IActiveSpawnRegistry
{
    void Register(Guid encounterId, IReadOnlyList<SpawnInstance> instances);

    IReadOnlyList<SpawnInstance> Release(Guid encounterId);

    int Count { get; }
}

public sealed class ActiveSpawnRegistry : IActiveSpawnRegistry
{
    private readonly ConcurrentDictionary<Guid, IReadOnlyList<SpawnInstance>> _active = new();

    public void Register(Guid encounterId, IReadOnlyList<SpawnInstance> instances) => _active[encounterId] = instances;

    public IReadOnlyList<SpawnInstance> Release(Guid encounterId) =>
        _active.TryRemove(encounterId, out var instances) ? instances : [];

    public int Count => _active.Count;
}
