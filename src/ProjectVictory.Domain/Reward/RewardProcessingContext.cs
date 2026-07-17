using System;

namespace ProjectVictory.Domain.Reward;

public sealed record RewardProcessingContext(
    Guid CombatId,
    Guid CharacterId);