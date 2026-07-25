namespace ProjectVictory.Application.Hunt;

public sealed record StopHuntSessionCommand(
    Guid HuntSessionId);
