namespace ProjectVictory.Application.Combat;

public interface ICombatResolver
{
    CombatResult Resolve(
        CombatRequest request);
}