namespace ProjectVictory.Domain.Reward;

public sealed record RewardDrop(
    Guid ItemDefinitionId,
    string ItemName);



public sealed record RewardResult(
    Guid CharacterId,
    Guid CombatId,
    bool Processed,
    int Experience,
    int Gold,
    IReadOnlyList<RewardDrop> Drops);