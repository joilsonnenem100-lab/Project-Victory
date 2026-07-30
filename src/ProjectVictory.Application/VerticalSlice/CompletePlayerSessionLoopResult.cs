using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CompletePlayerSessionLoopResult(
    Guid CharacterId,
    Guid RuntimeId,
    Guid SessionId,
    bool Completed);
