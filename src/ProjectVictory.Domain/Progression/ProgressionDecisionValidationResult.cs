using System;

namespace ProjectVictory.Domain.Progression;

public sealed record ProgressionDecisionValidationResult(
    Guid CharacterId,
    bool Valid);