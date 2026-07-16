namespace ProjectVictory.Application.Combat;

public sealed record ApplyCombatResultResult(
    Guid CombatContextId,
    bool Applied);