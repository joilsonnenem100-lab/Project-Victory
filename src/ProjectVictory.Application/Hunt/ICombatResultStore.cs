using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Hunt;

public interface ICombatResultStore
{
    void Store(
        CombatResult result);


    CombatResult Get(
        Guid huntId);
}