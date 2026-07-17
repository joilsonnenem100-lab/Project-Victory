using System;

namespace ProjectVictory.Application.Reward;

public sealed record ProcessRewardCommand(
    Guid CombatId,
    Guid CharacterId);