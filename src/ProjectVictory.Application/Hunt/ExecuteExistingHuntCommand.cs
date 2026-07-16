namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteExistingHuntCommand(
    Guid HuntId,
    Guid EnemyId);