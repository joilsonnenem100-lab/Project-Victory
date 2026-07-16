using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessExperienceCommand(
    Guid CharacterId,
    int ExperienceAmount);