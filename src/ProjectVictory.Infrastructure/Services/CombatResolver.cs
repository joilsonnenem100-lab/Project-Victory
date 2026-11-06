using ProjectVictory.Application.Combat;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Infrastructure.Services;

// PV-COMBAT-012: substitui o stub. As regras de ação ficam aqui, fora do Combat Orchestrator
// (ADR-051): dano pela IDamageFormula, custo de mana da habilidade e efeito dos suprimentos.
public sealed class CombatResolver : ICombatResolver
{
    private readonly IDamageFormula _damageFormula;
    private readonly ISupplyEffects _supplyEffects;

    public CombatResolver(
        IDamageFormula damageFormula,
        ISupplyEffects supplyEffects)
    {
        _damageFormula = damageFormula;
        _supplyEffects = supplyEffects;
    }

    // Contrato antigo (fluxo gameplay/action), mantido pela Regra Aditiva: o atacante vence.
    // Não é o combate real do jogo, que passa por ResolveAction (registrado no contrato).
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
        ArgumentNullException.ThrowIfNull(random);

        switch (action.Kind)
        {
            case CombatActionKind.BasicAttack:
                return new CombatActionOutcome(
                    _damageFormula.RollDamage(action.Actor, action.Target, action.Kind, random), 0, 0, 0, true);

            case CombatActionKind.ClassAbility:
                var cost = action.Actor.Profile.AbilityManaCost;

                // Sem mana, a ação não acontece e nenhum número aleatório é consumido.
                if (cost <= 0 || action.Actor.CurrentMana < cost)
                {
                    return CombatActionOutcome.NotExecuted;
                }

                return new CombatActionOutcome(
                    _damageFormula.RollDamage(action.Actor, action.Target, action.Kind, random), 0, 0, cost, true);

            case CombatActionKind.UseHealthPotion:
                return new CombatActionOutcome(0, _supplyEffects.HealthPotionHealing, 0, 0, true);

            case CombatActionKind.UseManaPotion:
                return new CombatActionOutcome(0, 0, _supplyEffects.ManaPotionRestore, 0, true);

            default:
                return CombatActionOutcome.NotExecuted;
        }
    }
}
