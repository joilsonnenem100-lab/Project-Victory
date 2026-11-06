using ProjectVictory.Domain.Combat.TurnBased;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-012/013: Secondary Action padrão (ADR-006).
// - Jogador: uma das 4 skills da classe, escolhida pela SkillSelectionPolicy (dados). Antes do
//   PV-COMBAT-013 usava a "habilidade inicial" (ClassAbility) sempre que tinha mana.
// - Monstro: a Monster AI orientada a dados (PV-MONSTER-010, ADR-005).
public sealed class DefaultCombatActionSelector : ICombatActionSelector
{
    private readonly ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy _policy;

    private readonly IMonsterAI _monsterAI;

    public DefaultCombatActionSelector()
        : this(ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy.Default)
    {
    }

    // Compatibilidade/testes de unidade: monstros só com ataque básico. Produção (DI) usa o construtor
    // com IMonsterAI, que lê os dados do catálogo.
    public DefaultCombatActionSelector(ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy policy)
        : this(policy, new DataDrivenMonsterAI(BasicAttackOnlyBehaviorProvider.Instance))
    {
    }

    // PV-MONSTER-010: a Monster AI escolhe a Secondary Action dos monstros.
    public DefaultCombatActionSelector(
        ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy policy,
        IMonsterAI monsterAI)
    {
        _policy = policy;
        _monsterAI = monsterAI ?? throw new ArgumentNullException(nameof(monsterAI));
    }

    public CombatActionKind? SelectSecondaryAction(
        CombatSession session,
        Combatant actor) =>
        SelectSecondaryChoice(session, actor)?.Kind;

    // PV-COMBAT-013 — *** PROVISÓRIO ***: jogador usa skill pela SkillSelectionPolicy (área com 2+
    // inimigos vivos, alvo único com 1, se houver mana; senão nenhuma). Monstros: Monster AI (dados).
    public CombatTurnChoice? SelectSecondaryChoice(
        CombatSession session,
        Combatant actor)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(actor);

        if (actor.Side == CombatSide.Monster)
        {
            return _monsterAI.SelectSecondaryChoice(session, actor);
        }

        if (actor.Side != CombatSide.Player)
        {
            return null;
        }

        var aliveEnemies = session.Monsters.Count(m => !m.IsDead);
        var skill = _policy.Choose(actor.Skills, aliveEnemies, actor.CurrentMana);

        return skill is null ? null : new CombatTurnChoice(CombatActionKind.UseSkill, skill);
    }
}
