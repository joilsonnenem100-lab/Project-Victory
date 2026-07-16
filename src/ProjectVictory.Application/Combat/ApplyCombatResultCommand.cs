namespace ProjectVictory.Application.Combat;

public sealed record ApplyCombatResultCommand(
    Guid CombatContextId,
    Guid ResolutionId);