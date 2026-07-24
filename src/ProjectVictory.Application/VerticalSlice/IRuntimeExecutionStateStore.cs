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
}