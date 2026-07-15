namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteNextHuntResult(
    Guid HuntId,
    bool Completed,
    int Experience,
    int Gold,
    int Level);