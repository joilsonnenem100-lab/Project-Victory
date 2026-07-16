namespace ProjectVictory.Application.Hunt;

public sealed record ProcessHuntExecutionResult(
    Guid HuntId,
    bool Completed);