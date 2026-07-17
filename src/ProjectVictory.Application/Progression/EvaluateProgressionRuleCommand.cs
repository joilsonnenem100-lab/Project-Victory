using System;

namespace ProjectVictory.Application.Progression;

public sealed record EvaluateProgressionRuleCommand(
    Guid CharacterId,
    int ExperienceAmount);