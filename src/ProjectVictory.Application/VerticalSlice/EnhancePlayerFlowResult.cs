using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record EnhancePlayerFlowResult(
    Guid CharacterId,
    bool Enhanced);