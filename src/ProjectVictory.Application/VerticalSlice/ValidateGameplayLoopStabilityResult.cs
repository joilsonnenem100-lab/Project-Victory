using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateGameplayLoopStabilityResult(
    Guid CharacterId,
    bool Stable);