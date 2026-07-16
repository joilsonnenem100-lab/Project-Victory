namespace ProjectVictory.Application.Reward;

public sealed record ApplyRewardResult(
    Guid CharacterId,
    bool Applied);