using System;

namespace ProjectVictory.Domain.Combat.TurnBased;

// ADR-006: todo turno = Basic Attack (obrigatório) + Secondary Action (opcional).
public enum CombatActionKind
{
    BasicAttack,
    ClassAbility,
    UseHealthPotion,
    UseManaPotion,
    // PV-COMBAT-013: uma das 4 skills da classe (ver ClassSkillTable).
    UseSkill
}

public sealed record CombatTurnAction(
    Combatant Actor,
    Combatant Target,
    CombatActionKind Kind,
    // PV-COMBAT-013: a skill usada (só com Kind = UseSkill).
    ProjectVictory.Domain.Combat.Rules.SkillDefinition? Skill = null);

// Resultado calculado pelas regras (ICombatResolver/IDamageFormula); o orquestrador só aplica.
public sealed record CombatActionOutcome(
    int Damage,
    int Healing,
    int ManaRestored,
    int ManaCost,
    bool Executed,
    // PV-COMBAT-013 (DEC-20261106-16): tipo do dano; hoje sempre físico, extensível para elementos.
    ProjectVictory.Domain.Combat.Rules.DamageType DamageType = ProjectVictory.Domain.Combat.Rules.DamageType.Physical)
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
    int ActorHealthAfter,
    // PV-COMBAT-013: nome da skill usada (null para outras ações).
    string? SkillName = null);

public enum CombatSessionOutcome
{
    Ongoing,
    Victory,
    Defeat
}
