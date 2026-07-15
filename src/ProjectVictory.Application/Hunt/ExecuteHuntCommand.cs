namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteHuntCommand(
    Guid CharacterId,
    Guid EnemyId);