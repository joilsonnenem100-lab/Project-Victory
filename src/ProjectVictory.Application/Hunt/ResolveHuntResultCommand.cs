namespace ProjectVictory.Application.Hunt;

public sealed record ResolveHuntResultCommand(
    Guid HuntId,
    bool Success);