using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-012: Secondary Action padrão (ADR-006).
// - Jogador: usa a habilidade inicial da classe se tiver mana. *** PROVISÓRIO *** — a política do
//   jogador vira configuração própria; poções entram no PV-SUPPLY-010.
// - Monstro: nenhuma (Monster AI é o PV-MONSTER-010, M4).
public sealed class DefaultCombatActionSelector : ICombatActionSelector
{
    public CombatActionKind? SelectSecondaryAction(
        CombatSession session,
        Combatant actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        if (actor.Side != CombatSide.Player)
        {
            return null;
        }

        var cost = actor.Profile.AbilityManaCost;

        return cost > 0 && actor.CurrentMana >= cost
            ? CombatActionKind.ClassAbility
            : null;
    }
}
