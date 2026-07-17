using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ConfirmPlayableVerticalSliceResult(
    Guid CharacterId,
    bool Confirmed);