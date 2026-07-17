using System;

namespace ProjectVictory.Domain.Progression;

public sealed record GrowthResult(
    Guid CharacterId,
    int ExperienceAmount,
    int CurrentLevel,
    bool Valid);