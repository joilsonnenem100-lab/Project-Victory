namespace ProjectVictory.Application.Hunt;

public sealed record ExecuteHuntResult(
    Guid HuntId,
    bool Completed,
    int Experience,
    int Gold,
    int Level);