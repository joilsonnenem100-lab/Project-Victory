namespace ProjectVictory.Application.Combat;

public sealed record CreateCombatContextResult(
    Guid CombatContextId,
    Guid HuntId);