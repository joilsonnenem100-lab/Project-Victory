namespace ProjectVictory.Application.Hunt;

public sealed record FinalizeHuntResult(
    Guid HuntId,
    bool Finalized);