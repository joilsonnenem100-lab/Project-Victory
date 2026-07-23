namespace ProjectVictory.Application.Reward;

public sealed record RewardResult(
    int Experience,
    int Gold,
    IReadOnlyList<RewardDrop> Drops);



public sealed record RewardDrop(
    Guid ItemDefinitionId,
    string ItemName);