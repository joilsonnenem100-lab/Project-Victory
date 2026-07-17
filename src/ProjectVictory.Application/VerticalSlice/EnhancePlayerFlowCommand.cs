using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record EnhancePlayerFlowCommand(
    Guid CharacterId);