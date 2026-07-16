namespace ProjectVictory.Application.Combat;

public sealed record FinalizeCombatResult(
    Guid CombatContextId,
    bool Finalized);