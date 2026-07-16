using System;

namespace ProjectVictory.Application.Progression;

public sealed record ApplyProgressionRuleCommand(
    Guid CharacterId,
    int ExperienceAmount);