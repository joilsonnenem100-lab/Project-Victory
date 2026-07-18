using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateFirstPlayableReadinessResult(
    Guid CharacterId,
    bool Ready);