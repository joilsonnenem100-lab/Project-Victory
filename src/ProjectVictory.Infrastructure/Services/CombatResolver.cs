using ProjectVictory.Application.Combat;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Infrastructure.Services;

public sealed class CombatResolver : ICombatResolver
{
    // Dano fixo do stub anterior (AttackEncounterHandler.AttackDamage = 5). PV-COMBAT-012 o
    // substitui pela IDamageFormula.
    private const int StubBasicAttackDamage = 5;

    public CombatResult Resolve(
        CombatRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CombatResult(
            request.AttackerId,
            true);
    }

    public CombatActionOutcome ResolveAction(
        CombatTurnAction action,
        IRandomSource random)
    {
        ArgumentNullException.ThrowIfNull(action);

        return action.Kind == CombatActionKind.BasicAttack
            ? new CombatActionOutcome(StubBasicAttackDamage, 0, 0, 0, true)
            : CombatActionOutcome.NotExecuted;
    }
}
