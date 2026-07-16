using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ExecuteVerticalSliceResult(
    Guid CharacterId,
    bool Completed);