using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-012/013: Secondary Action padrão (ADR-006).
// - Jogador: uma das 4 skills da classe, escolhida pela SkillSelectionPolicy (dados). Antes do
//   PV-COMBAT-013 usava a "habilidade inicial" (ClassAbility) sempre que tinha mana.
// - Monstro: nenhuma (Monster AI é o PV-MONSTER-010, M4).
public sealed class DefaultCombatActionSelector : ICombatActionSelector
{
    private readonly ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy _policy;

    public DefaultCombatActionSelector()
        : this(ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy.Default)
    {
    }

    public DefaultCombatActionSelector(ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy policy)
    {
        _policy = policy;
    }

    public CombatActionKind? SelectSecondaryAction(
        CombatSession session,
        Combatant actor) =>
        SelectSecondaryChoice(session, actor)?.Kind;

    // PV-COMBAT-013 — *** PROVISÓRIO ***: jogador usa skill pela SkillSelectionPolicy (área com 2+
    // inimigos vivos, alvo único com 1, se houver mana; senão nenhuma). Monstros: nenhuma (M4).
    public CombatTurnChoice? SelectSecondaryChoice(
        CombatSession session,
        Combatant actor)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(actor);

        if (actor.Side != CombatSide.Player)
        {
            return null;
        }

        var aliveEnemies = session.Monsters.Count(m => !m.IsDead);
        var skill = _policy.Choose(actor.Skills, aliveEnemies, actor.CurrentMana);

        return skill is null ? null : new CombatTurnChoice(CombatActionKind.UseSkill, skill);
    }
}
