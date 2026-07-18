using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ApproveVerticalSliceFinalResult(
    Guid CharacterId,
    bool Approved);