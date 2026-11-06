using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public interface IRuntimeExecutionStateStore
{
    void Store(
        RuntimeExecutionContext context);

    RuntimeExecutionContext Get(
        Guid runtimeId);

    RuntimeExecutionContext GetByCharacterId(
        Guid characterId);

    // PV-STATE-001: permite recusar a criação de um runtime com id já em uso.
    bool Contains(
        Guid runtimeId);

    // PV-HUNT-012: consulta sem exceção (o personagem pode não ter Runtime carregado).
    bool TryGetByCharacterId(
        Guid characterId,
        out RuntimeExecutionContext context);
}
