using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProgressionRequest(
    Guid CharacterId,
    int ExperienceGain);