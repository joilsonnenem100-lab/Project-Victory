using System;

namespace ProjectVictory.Application.Architecture;

public sealed record ApprovePostSliceArchitectureCommand(
    Guid ApprovalId,
    bool ImpactApproved,
    bool ConsistencyValidated);