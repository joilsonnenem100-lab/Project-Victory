namespace ProjectVictory.Application.Combat;

public sealed record FinalizeCombatCommand(
    Guid CombatContextId);