namespace ProjectVictory.Application.Hunt;

public sealed record CancelHuntResult(
    Guid HuntId,
    bool Cancelled);