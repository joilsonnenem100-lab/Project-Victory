using System;

namespace ProjectVictory.Domain.Progression;

public sealed record ProgressionFinalState(
    Guid CharacterId,
    bool Completed);