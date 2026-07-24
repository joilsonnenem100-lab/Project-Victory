using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CreateRuntimeExecutionCommand(
    Guid RuntimeId,
    Guid CharacterId);