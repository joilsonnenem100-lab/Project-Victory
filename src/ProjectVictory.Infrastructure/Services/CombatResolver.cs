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

            // PV-COMBAT-013: skill de classe (dados em ClassSkillTable). Sem mana, não executa e não
            // consome aleatoriedade. Tipo de dano da skill (hoje só físico).
            case CombatActionKind.UseSkill when action.Skill is { } skill:
                if (action.Actor.CurrentMana < skill.ManaCost)
                {
                    return CombatActionOutcome.NotExecuted;
                }

                return new CombatActionOutcome(
                    _damageFormula.RollDamage(action.Actor, action.Target, skill.DamageMultiplier, random),
                    0,
                    0,
                    skill.ManaCost,
                    true,
                    skill.DamageType);

            // PV-COMBAT-014 (PO-53): spell de cura da vocação. Regra AQUI (ADR-051), não no orquestrador:
            // cura = % da vida MÁXIMA pela faixa de Level (dados), limitada ao que falta para o máximo;
            // gasta a Mana da spell. Sem spell, morto ou sem Mana: não executa. Não consome aleatoriedade
            // (o combate segue determinístico com a semente do encontro).
            case CombatActionKind.UseHealingSpell:
                if (action.Actor.HealingSpell is not { } spell
                    || action.Actor.IsDead
                    || action.Actor.CurrentMana < spell.ManaCost)
                {
                    return CombatActionOutcome.NotExecuted;
                }

                var missing = action.Actor.MaximumHealth - action.Actor.CurrentHealth;
                var healing = Math.Min(spell.RawHealingFor(action.Actor.Level, action.Actor.MaximumHealth), missing);

                return new CombatActionOutcome(0, healing, 0, spell.ManaCost, true);

            case CombatActionKind.UseHealthPotion:
                return new CombatActionOutcome(0, _supplyEffects.HealthPotionHealing, 0, 0, true);

            case CombatActionKind.UseManaPotion:
                return new CombatActionOutcome(0, 0, _supplyEffects.ManaPotionRestore, 0, true);

            default:
                return CombatActionOutcome.NotExecuted;
        }
    }

    // Revisão do PV-COMBAT-013 (B1/B2): skill em área atinge TODOS os oponentes vivos e custa a
    // mana uma vez. A mana é conferida UMA vez, antes de qualquer número aleatório (determinismo);
    // depois, um sorteio de dano por alvo, na ordem dos oponentes.
    public SkillResolution ResolveSkill(
        Combatant actor,
        SkillDefinition skill,
        IReadOnlyList<Combatant> aliveOpponents,
        IRandomSource random)
    {
        if (skill.Targeting != SkillTargeting.Area
            || aliveOpponents.Count == 0
            || actor.CurrentMana < skill.ManaCost)
        {
            return SkillResolution.NotExecuted;
        }

        var hits = aliveOpponents
            .Select(target => new SkillHit(
                target,
                new CombatActionOutcome(
                    _damageFormula.RollDamage(actor, target, skill.DamageMultiplier, random),
                    0,
                    0,
                    0,
                    true,
                    skill.DamageType)))
            .ToList();

        return new SkillResolution(true, skill.ManaCost, hits);
    }
}
