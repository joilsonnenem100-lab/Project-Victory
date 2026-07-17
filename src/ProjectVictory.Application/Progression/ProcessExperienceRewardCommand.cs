using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessExperienceRewardCommand(
    Guid CharacterId,
    int ExperienceAmount);