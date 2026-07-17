using System;

namespace ProjectVictory.Application.Reward;

public sealed record CreateRewardContentResult(
    Guid CombatResultId,
    Guid RewardContentId,
    bool Created);