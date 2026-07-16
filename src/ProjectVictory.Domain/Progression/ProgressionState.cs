using System;

namespace ProjectVictory.Domain.Progression;

public sealed record ProgressionState(
    Guid CharacterId,
    int ExperienceAmount,
    bool Completed);