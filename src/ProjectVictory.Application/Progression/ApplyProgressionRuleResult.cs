using System;

namespace ProjectVictory.Application.Progression;

public sealed record ApplyProgressionRuleResult(
    Guid CharacterId,
    bool Accepted);