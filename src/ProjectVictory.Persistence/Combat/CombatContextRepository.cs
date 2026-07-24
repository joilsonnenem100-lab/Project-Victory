using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Persistence.Combat;

public sealed class CombatContextRepository
    : ICombatContextRepository
{
    private readonly List<CombatContext> _contexts = [];

    public Task AddAsync(
        CombatContext combatContext,
        CancellationToken cancellationToken = default)
    {
        _contexts.Add(combatContext);

        return Task.CompletedTask;
    }

    public Task<CombatContext?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var context =
            _contexts.FirstOrDefault(
                x => x.Id == id);

        return Task.FromResult(context);
    }
}