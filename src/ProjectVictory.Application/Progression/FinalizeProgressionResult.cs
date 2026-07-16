using System;

namespace ProjectVictory.Application.Progression;

public sealed record FinalizeProgressionResult(
    Guid CharacterId,
    bool Completed);