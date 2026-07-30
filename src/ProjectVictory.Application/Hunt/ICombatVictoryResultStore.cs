using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Hunt;

public interface ICombatVictoryResultStore
{
    void Store(
        CombatVictoryResult result);

    CombatVictoryResult Get(
        Guid huntId);
}
