namespace ProjectVictory.Application.Hunt;

public sealed record StartHuntResult(
    Guid HuntId,
    Guid CharacterId);