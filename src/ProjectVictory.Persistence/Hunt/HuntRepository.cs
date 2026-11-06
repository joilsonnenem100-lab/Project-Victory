using System.Collections.Concurrent;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Persistence.Hunt;

// PV-STATE-001: estado TEMPORÁRIO (execução da Hunt; ADR-057 — a Hunt nunca é recuperada),
// mantido em memória num singleton compartilhado entre requisições. Antes era List<> sem
// sincronização; agora é ConcurrentDictionary (leituras e escritas concorrentes seguras).
public sealed class HuntRepository : IHuntRepository
{
    private readonly ConcurrentDictionary<Guid, Domain.Hunt.Hunt> _hunts = new();

    public Task AddAsync(
        Domain.Hunt.Hunt hunt,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(hunt);

        if (!_hunts.TryAdd(hunt.Id, hunt))
        {
            throw new InvalidOperationException(
                "Hunt already exists.");
        }

        return Task.CompletedTask;
    }

    public Task<Domain.Hunt.Hunt?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _hunts.TryGetValue(id, out var hunt);

        return Task.FromResult(hunt);
    }
}
