using System;

namespace ProjectVictory.Application.Production;

public sealed class ApproveProductionFoundationHandler
{
    public ApproveProductionFoundationResult Handle(
        ApproveProductionFoundationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var approved =
            command.ApprovalId != Guid.Empty &&
            command.ReadinessValidated &&
            command.DeploymentValidated;

        return new ApproveProductionFoundationResult(
            command.ApprovalId,
            approved);
    }
}