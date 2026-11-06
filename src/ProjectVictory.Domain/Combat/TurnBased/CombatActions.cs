using System;

namespace ProjectVictory.Domain.Combat.TurnBased;

// ADR-006: todo turno = Basic Attack (obrigatório) + Secondary Action (opcional).
public enum CombatActionKind
{
    BasicAttack,
    ClassAbility,
    UseHealthPotion,
    UseManaPotion
}

public sealed record CombatTurnAction(
    Combatant Actor,
    Combatant Target,
    CombatActionKind Kind);

// Resultado calculado pelas regras (ICombatResolver/IDamageFormula); o orquestrador só aplica.
public sealed record CombatActionOutcome(
    int Damage,
    int Healing,
    int ManaRestored,
    int ManaCost,
    bool Executed)
{
    public static CombatActionOutcome NotExecuted { get; } = new(0, 0, 0, 0, false);
}

public sealed record CombatTurnLogEntry(
    int Turn,
    Guid ActorId,
    string ActorName,
    Guid TargetId,
    string TargetName,
    CombatActionKind Kind,
    int Damage,
    int Healing,
    int TargetHealthAfter,
    int ActorHealthAfter);

public enum CombatSessionOutcome
{
    Ongoing,
    Victory,
    Defeat
}
