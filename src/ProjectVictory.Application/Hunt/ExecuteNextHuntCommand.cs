namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteNextHuntCommand(
    Guid CharacterId,
    Guid EnemyId);