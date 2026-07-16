namespace ProjectVictory.Application.Reward;

public sealed record ApplyRewardCommand(
    Guid CharacterId,
    RewardRequest Request);