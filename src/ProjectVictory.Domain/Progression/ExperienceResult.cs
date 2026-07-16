using System;

namespace ProjectVictory.Domain.Progression;

public sealed record ExperienceResult(
    Guid CharacterId,
    int ExperienceAmount,
    bool Processed);