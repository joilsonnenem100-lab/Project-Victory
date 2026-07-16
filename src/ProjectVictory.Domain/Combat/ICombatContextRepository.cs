namespace ProjectVictory.Domain.Combat;

public interface ICombatContextRepository
{
    Task AddAsync(
        CombatContext combatContext,
        CancellationToken cancellationToken = default);

    Task<CombatContext?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}