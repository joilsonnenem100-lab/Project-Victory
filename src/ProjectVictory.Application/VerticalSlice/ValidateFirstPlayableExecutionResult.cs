using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateFirstPlayableExecutionResult(
    Guid CharacterId,
    bool Ready);