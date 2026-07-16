using System;

namespace ProjectVictory.Application.Progression;

public sealed record GenerateProgressionResultCommand(
    Guid CharacterId,
    int ExperienceAmount);