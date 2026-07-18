using System;

namespace ProjectVictory.Application.Architecture;

public sealed record AnalyzeArchitectureImpactResult(
    Guid ChangeRequestId,
    bool Validated);