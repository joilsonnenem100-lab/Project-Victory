using System;

namespace ProjectVictory.Application.Reward;

public sealed record CreateRewardContentCommand(
    Guid CombatResultId,
    Guid RewardContentId);