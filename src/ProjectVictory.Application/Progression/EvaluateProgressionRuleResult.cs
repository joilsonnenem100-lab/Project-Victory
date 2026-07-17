using System;

namespace ProjectVictory.Application.Progression;

public sealed record EvaluateProgressionRuleResult(
    Guid CharacterId,
    bool CanEvolve);