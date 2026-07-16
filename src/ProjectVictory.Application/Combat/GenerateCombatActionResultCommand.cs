namespace ProjectVictory.Application.Combat;

public sealed record GenerateCombatActionResultCommand(
    Guid CombatContextId,
    Guid ActionId);