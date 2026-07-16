using System;

namespace ProjectVictory.Application.Reward;

public sealed record ForwardRewardToProgressionResult(
    Guid CharacterId,
    bool Forwarded);