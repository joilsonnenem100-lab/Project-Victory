using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CompletePlayerSessionLoopCommand(
    Guid CharacterId,
    Guid RuntimeId,
    Guid SessionId);
