using System;

namespace ProjectVictory.Application.Architecture;

public sealed record AnalyzeArchitectureImpactCommand(
    Guid ChangeRequestId,
    bool ChangeIdentified,
    bool ImpactAnalyzed);