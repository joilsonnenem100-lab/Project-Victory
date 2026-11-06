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
}
