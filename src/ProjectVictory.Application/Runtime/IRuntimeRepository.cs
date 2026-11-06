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
    // Revisão (ressalva 4): sem corpo padrão — toda implementação precisa gravar de fato.
    Task SaveCheckpointAsync(
        Guid runtimeId,
        int health,
        int mana,
        CancellationToken cancellationToken = default);

    // PV-RUNTIME-010 (ADR-057 v1.2): grava a última Hunt escolhida no checkpoint do Runtime.
    Task SaveLastHuntAsync(
        Guid runtimeId,
        string huntArea,
        CancellationToken cancellationToken = default);
}
