namespace ProjectVictory.Application.Combat;

public sealed record ExecuteCombatActionCommand(
    Guid CombatContextId,
    Guid ActionId);