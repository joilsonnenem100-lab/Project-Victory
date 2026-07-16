using System;

namespace ProjectVictory.Application.Progression;

public sealed record GenerateExperienceResultCommand(
    Guid CharacterId,
    int ExperienceAmount);