using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record RefineGameplayFlowCommand(
    Guid CharacterId);