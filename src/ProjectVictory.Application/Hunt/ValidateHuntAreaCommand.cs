using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateHuntAreaCommand(
    Guid WorldId,
    Guid HuntAreaId,
    bool HuntExecuted,
    bool CombatCompleted,
    bool RewardProcessed,
    bool CharacterUpdated);