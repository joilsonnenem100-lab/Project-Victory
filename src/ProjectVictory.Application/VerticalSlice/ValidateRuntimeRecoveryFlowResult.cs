using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateRuntimeRecoveryFlowResult(
    Guid CharacterId,
    bool Recovered);