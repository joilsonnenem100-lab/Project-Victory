using System;

namespace ProjectVictory.Application.Production;

public sealed record AnalyzeProductionReadinessResult(
    Guid AnalysisId,
    bool Ready);