using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionRecoveryResult(
    Guid CharacterId,
    bool Recovered);