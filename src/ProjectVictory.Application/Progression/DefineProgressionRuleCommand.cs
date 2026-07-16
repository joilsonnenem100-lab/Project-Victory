using System;

namespace ProjectVictory.Application.Progression;

public sealed record DefineProgressionRuleCommand(
    Guid CharacterId,
    int ExperienceAmount);