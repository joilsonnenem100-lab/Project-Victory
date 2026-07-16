using System;

namespace ProjectVictory.Application.Progression;

public sealed record CalculateLevelCommand(
    Guid CharacterId,
    int ExperienceAmount);