using System.Collections.Concurrent;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Persistence.Combat;

// PV-STATE-001: estado TEMPORÁRIO de combate, em memória num singleton. Antes era List<>
// sem sincronização; agora é ConcurrentDictionary.
public sealed class CombatContextRepository
    : ICombatContextRepository
{
    private readonly ConcurrentDictionary<Guid, CombatContext> _contexts = new();

    public Task AddAsync(
        CombatContext combatContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(combatContext);

        if (!_contexts.TryAdd(combatContext.Id, combatContext))
        {
            throw new InvalidOperationException(
                "Combat context already exists.");
        }

        return Task.CompletedTask;
    }

    public Task<CombatContext?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _contexts.TryGetValue(id, out var context);

        return Task.FromResult(context);
    }
}
