using System;

namespace ProjectVictory.Application.Production;

public sealed record ApproveProductionFoundationCommand(
    Guid ApprovalId,
    bool ReadinessValidated,
    bool DeploymentValidated);