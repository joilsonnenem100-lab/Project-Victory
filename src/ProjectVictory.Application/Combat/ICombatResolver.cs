using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Application.Combat;

public interface ICombatResolver
{
    // Contrato anterior (combate inteiro por ids; usado pelo fluxo de gameplay/action). Mantido
    // pela Regra Aditiva.
    CombatResult Resolve(
        CombatRequest request);

    // PV-COMBAT-010/012: resolve UMA ação de um turno (dano, cura, custo de mana) pelas regras de
    // combate. É aqui, e não no Combat Orchestrator, que vivem as regras (ADR-051, ADR-006).
    CombatActionOutcome ResolveAction(
        CombatTurnAction action,
        IRandomSource random);

    // Revisão do PV-COMBAT-013 (B1/B2, ADR-051): regra de skill em área fica no resolver, não no
    // orquestrador. Recebe os oponentes vivos (dado da sessão) e devolve os alvos atingidos, o
    // resultado de cada um e o custo de mana ÚNICO. Padrão (aditivo, para resolvers que não
    // conhecem skills em área): não executa.
    SkillResolution ResolveSkill(
        Combatant actor,
        SkillDefinition skill,
        IReadOnlyList<Combatant> aliveOpponents,
        IRandomSource random) => SkillResolution.NotExecuted;
}

// Revisão do PV-COMBAT-013: resultado de uma skill (área) — um resultado por alvo; a mana é
// cobrada uma vez (ManaCost), nunca por alvo.
public sealed record SkillResolution(
    bool Executed,
    int ManaCost,
    IReadOnlyList<SkillHit> Hits)
{
    public static SkillResolution NotExecuted { get; } = new(false, 0, []);
}

public sealed record SkillHit(
    Combatant Target,
    CombatActionOutcome Outcome);
