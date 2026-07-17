using System;

namespace ProjectVictory.Application.Reward;

public sealed record RefineRewardProgressionFlowResult(
    Guid CharacterId,
    bool Validated);