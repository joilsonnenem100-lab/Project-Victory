using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record PrepareInternalPlaytestResult(
    Guid RuntimeId,
    bool Prepared);