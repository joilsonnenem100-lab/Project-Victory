using System;

namespace ProjectVictory.Application.Production;

public sealed class ValidateDeploymentHandler
{
    public ValidateDeploymentResult Handle(
        ValidateDeploymentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validated =
            command.DeploymentId != Guid.Empty &&
            command.BuildValidated &&
            command.DistributionValidated;

        return new ValidateDeploymentResult(
            command.DeploymentId,
            validated);
    }
}