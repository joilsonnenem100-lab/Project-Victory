namespace ProjectVictory.Application.Combat;

public sealed record GenerateCombatResolutionCommand(
    Guid CombatContextId,
    Guid ActionResultId);