using System;

namespace ProjectVictory.Application.Reward;

public sealed record ProcessRewardResult(
    Guid CharacterId,
    bool Processed);