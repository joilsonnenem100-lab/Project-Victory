namespace ProjectVictory.Application.Combat;

public sealed record GenerateCombatResolutionResult(
    Guid ResolutionId,
    Guid CombatContextId);