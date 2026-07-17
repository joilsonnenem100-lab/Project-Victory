using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record RefineGameplayFlowResult(
    Guid CharacterId,
    bool Validated);