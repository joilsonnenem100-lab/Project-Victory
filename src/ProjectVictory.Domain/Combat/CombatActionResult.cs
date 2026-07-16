namespace ProjectVictory.Domain.Combat;

public sealed record CombatActionResult(
    Guid CombatContextId,
    bool Processed);