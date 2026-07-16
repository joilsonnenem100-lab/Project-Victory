using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ExecuteVerticalSliceCommand(
    Guid CharacterId);