using System;

namespace ProjectVictory.Application.Reward;

public sealed record ForwardRewardToProgressionCommand(
    Guid CharacterId,
    RewardResult Reward);