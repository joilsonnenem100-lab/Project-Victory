using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessExperienceRewardResult(
    Guid CharacterId,
    int ExperienceAmount,
    bool Processed);