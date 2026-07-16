namespace ProjectVictory.Application.Hunt;

public sealed record ForwardHuntRewardResult(
    Guid HuntId,
    bool Forwarded);