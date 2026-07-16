using System;

namespace ProjectVictory.Domain.Progression;

public sealed record ProgressionRuleConfirmation(
    Guid CharacterId,
    bool Confirmed);