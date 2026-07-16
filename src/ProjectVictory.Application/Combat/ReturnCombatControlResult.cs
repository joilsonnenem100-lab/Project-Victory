namespace ProjectVictory.Application.Combat;

public sealed record ReturnCombatControlResult(
    Guid CombatContextId,
    Guid HuntId,
    bool Returned);