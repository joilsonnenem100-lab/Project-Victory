using System;

namespace ProjectVictory.Application.Production;

public sealed record ValidateDeploymentCommand(
    Guid DeploymentId,
    bool BuildValidated,
    bool DistributionValidated);