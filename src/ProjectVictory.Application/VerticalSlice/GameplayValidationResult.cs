using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record GameplayValidationResult(
    Guid CharacterId,
    bool SessionValid,
    bool HuntValid,
    bool CombatValid,
    bool RewardValid,
    bool ProgressionValid,
    bool Completed);