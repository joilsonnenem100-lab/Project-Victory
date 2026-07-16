using System;

namespace ProjectVictory.Application.Progression;

public sealed record ConfirmProgressionResult(
    Guid CharacterId,
    bool Confirmed);