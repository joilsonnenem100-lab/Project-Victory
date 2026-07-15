namespace ProjectVictory.Application.Game;

public sealed record ExecuteGameplayLoopCommand(
    Guid AccountId,
    Guid EnemyId);