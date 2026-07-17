using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CreateRuntimeExecutionResult(
    Guid RuntimeId,
    Guid CharacterId,
    bool Created);