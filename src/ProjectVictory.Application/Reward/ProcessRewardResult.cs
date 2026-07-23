namespace ProjectVictory.Application.Reward;

public sealed record ProcessRewardResult(
    Guid CharacterId,
    bool Processed,
    int Experience,
    int Gold,
    IReadOnlyList<RewardDrop> Drops);