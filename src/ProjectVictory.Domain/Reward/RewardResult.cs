namespace ProjectVictory.Domain.Reward;

public sealed record RewardResult(
    Guid CharacterId,
    Guid CombatId,
    bool Processed);