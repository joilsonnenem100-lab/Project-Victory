using System;

namespace ProjectVictory.Application.Production;

public sealed record AnalyzeProductionReadinessCommand(
    Guid AnalysisId,
    bool TechnicalRequirementsValidated,
    bool ExistingSystemsReady);