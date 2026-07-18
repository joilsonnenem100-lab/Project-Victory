using ProjectVictory.Application.Combat;

namespace ProjectVictory.Infrastructure.Services;

public sealed class CombatResolver : ICombatResolver
{
    public CombatResult Resolve(
        CombatRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CombatResult(
            request.AttackerId,
            true);
    }
}