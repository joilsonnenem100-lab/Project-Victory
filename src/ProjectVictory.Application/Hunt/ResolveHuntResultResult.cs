namespace ProjectVictory.Application.Hunt;

public sealed record ResolveHuntResultResult(
    Guid HuntId,
    bool Success,
    bool SendToReward);