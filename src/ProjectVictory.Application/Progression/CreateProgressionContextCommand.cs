using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateProgressionContextCommand(
    Guid CharacterId,
    int ExperienceAmount);