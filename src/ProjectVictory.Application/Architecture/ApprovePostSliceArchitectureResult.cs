using System;

namespace ProjectVictory.Application.Architecture;

public sealed record ApprovePostSliceArchitectureResult(
    Guid ApprovalId,
    bool Approved);