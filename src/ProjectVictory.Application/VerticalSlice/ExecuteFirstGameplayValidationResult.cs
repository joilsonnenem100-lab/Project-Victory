using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ExecuteFirstGameplayValidationResult(
    Guid CharacterId,
    bool Validated);