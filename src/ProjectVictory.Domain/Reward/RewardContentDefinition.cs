using System;

namespace ProjectVictory.Domain.Reward;

public sealed record RewardContentDefinition(
    Guid Id,
    Guid RewardId,
    string Name);