namespace ProjectVictory.Application.VerticalSlice;

public interface IRuntimeRepository
{
    Task AddAsync(
        Guid runtimeId,
        Guid characterId,
        int health,
        int mana,
        CancellationToken cancellationToken = default);

    Task<RuntimeRecoveryData?> GetByCharacterIdAsync(
        Guid characterId,
        CancellationToken cancellationToken = default);
}
