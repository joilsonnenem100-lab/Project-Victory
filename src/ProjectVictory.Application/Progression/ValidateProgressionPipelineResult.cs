using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionPipelineResult(
    Guid CharacterId,
    bool Validated);