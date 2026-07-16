using System;

namespace ProjectVictory.Application.Progression;

public sealed record ConfirmProgressionRuleResult(
    Guid CharacterId,
    bool Confirmed);