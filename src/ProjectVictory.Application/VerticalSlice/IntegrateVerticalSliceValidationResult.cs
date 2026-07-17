using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record IntegrateVerticalSliceValidationResult(
    Guid CharacterId,
    bool Validated,
    GameplayValidationResult Validation);