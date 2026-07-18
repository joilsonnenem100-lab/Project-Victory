using System;

namespace ProjectVictory.Application.Production;

public sealed class AnalyzeProductionReadinessHandler
{
    public AnalyzeProductionReadinessResult Handle(
        AnalyzeProductionReadinessCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ready =
            command.AnalysisId != Guid.Empty &&
            command.TechnicalRequirementsValidated &&
            command.ExistingSystemsReady;

        return new AnalyzeProductionReadinessResult(
            command.AnalysisId,
            ready);
    }
}