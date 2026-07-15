using System;

namespace ProjectVictory.Application.Reward;

public sealed record RewardRequest(
    Guid CharacterId,
    object SourceResult);