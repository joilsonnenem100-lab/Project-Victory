namespace ProjectVictory.Application.Combat;

public sealed record CreateCombatContextCommand(
    Guid HuntId,
    Guid AttackerId,
    Guid MonsterId);