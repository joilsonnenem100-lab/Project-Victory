using System;

namespace ProjectVictory.Application.Reward;

public sealed record ValidateRewardContentCommand(
    Guid CombatResultId,
    Guid RewardContentId,
    Guid CharacterId,
    bool RewardProcessed,
    bool CharacterUpdated);