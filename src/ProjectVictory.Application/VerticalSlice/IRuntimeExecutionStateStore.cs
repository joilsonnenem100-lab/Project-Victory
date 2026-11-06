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
}