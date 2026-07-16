using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record VerticalSliceFinalState(
    Guid CharacterId,
    bool FlowCompleted);