using System.Collections.Concurrent;
using ProjectVictory.Domain.Spawn;

namespace ProjectVictory.Application.Hunt;

// PV-HUNT-011 (checklist do PV-STATE-001 D3): serializa os comandos sobre a MESMA Hunt (ataque,
// saída manual, queda) — um por vez; Hunts diferentes seguem em paralelo.
// INVARIANTE (revisão do PV-HUNT-012, ressalva 6): Release descarta o semáforo de uma Hunt encerrada
// mesmo com o dono ainda dentro dele; quem chegar depois ganha um semáforo novo. Por isso TODO comando,
// depois de pegar o lock, reconfere o estado (Hunt Running / encounter presente) antes de agir.
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

// PV-HUNT-013 (DEC-20261106-46): serializa o INÍCIO de Hunt por personagem, para que a conferência
// "já há Hunt em andamento?" e a criação da nova sejam atômicas (dois inícios simultâneos → um 409).
// Um semáforo por personagem que já iniciou Hunt (memória limitada ao número de personagens).
public interface IHuntStartGate
{
    Task<IDisposable> AcquireAsync(Guid characterId, CancellationToken cancellationToken = default);
}

public sealed class HuntStartGate : IHuntStartGate
{
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _gates = new();

    public async Task<IDisposable> AcquireAsync(Guid characterId, CancellationToken cancellationToken = default)
    {
        var semaphore = _gates.GetOrAdd(characterId, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(cancellationToken);
        return new Releaser(semaphore);
    }

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
