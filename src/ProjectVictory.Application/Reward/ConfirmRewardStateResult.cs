namespace ProjectVictory.Application.Reward;

public sealed record ConfirmRewardStateResult(
    Guid CharacterId,
    bool Valid);