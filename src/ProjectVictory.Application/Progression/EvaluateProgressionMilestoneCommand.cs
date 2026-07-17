using System;

namespace ProjectVictory.Application.Progression;

public sealed record EvaluateProgressionMilestoneCommand(
    Guid CharacterId,
    int ExperienceAmount);