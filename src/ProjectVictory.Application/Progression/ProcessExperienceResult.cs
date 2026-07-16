using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessExperienceResult(
    Guid CharacterId,
    int ExperienceAmount,
    bool Processed);