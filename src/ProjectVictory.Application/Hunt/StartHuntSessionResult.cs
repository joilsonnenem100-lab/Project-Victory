namespace ProjectVictory.Application.Hunt;

public sealed record StartHuntSessionResult(
    Guid HuntSessionId,
    Guid CharacterId);
