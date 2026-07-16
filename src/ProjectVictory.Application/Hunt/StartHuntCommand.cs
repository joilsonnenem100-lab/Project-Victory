namespace ProjectVictory.Application.Hunt;

public sealed record StartHuntCommand(
    Guid CharacterId,
    Guid EnemyId);