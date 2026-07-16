using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ConsolidateVerticalSliceCommand(
    Guid CharacterId,
    bool FlowCompleted);