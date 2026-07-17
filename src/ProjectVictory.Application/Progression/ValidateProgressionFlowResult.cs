using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionFlowResult(
    Guid CharacterId,
    bool Validated);