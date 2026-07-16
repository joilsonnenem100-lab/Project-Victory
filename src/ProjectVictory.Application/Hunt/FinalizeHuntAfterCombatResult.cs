namespace ProjectVictory.Application.Hunt;

public sealed record FinalizeHuntAfterCombatResult(
    Guid HuntId,
    bool Finalized);