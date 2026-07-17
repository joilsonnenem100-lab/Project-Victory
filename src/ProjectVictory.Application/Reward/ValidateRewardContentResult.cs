using System;

namespace ProjectVictory.Application.Reward;

public sealed record ValidateRewardContentResult(
    Guid RewardContentId,
    bool Valid);