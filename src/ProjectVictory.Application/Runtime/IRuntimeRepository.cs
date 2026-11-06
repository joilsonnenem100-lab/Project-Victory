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

    // PV-PERSIST-011: grava a cópia do Runtime (vida/mana atuais) num checkpoint (Handbook §19).
    // Padrão: sem efeito (dublês de teste); o RuntimeRepository (EF) grava.
    Task SaveCheckpointAsync(
        Guid runtimeId,
        int health,
        int mana,
        CancellationToken cancellationToken = default) => Task.CompletedTask;
}
