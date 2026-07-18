using System;

namespace ProjectVictory.Application.Production;

public sealed record ValidateDeploymentResult(
    Guid DeploymentId,
    bool Validated);