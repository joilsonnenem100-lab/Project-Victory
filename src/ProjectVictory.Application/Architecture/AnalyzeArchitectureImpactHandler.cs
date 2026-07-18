using System;

namespace ProjectVictory.Application.Architecture;

public sealed class AnalyzeArchitectureImpactHandler
{
    public AnalyzeArchitectureImpactResult Handle(
        AnalyzeArchitectureImpactCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validated =
            command.ChangeRequestId != Guid.Empty &&
            command.ChangeIdentified &&
            command.ImpactAnalyzed;

        return new AnalyzeArchitectureImpactResult(
            command.ChangeRequestId,
            validated);
    }
}