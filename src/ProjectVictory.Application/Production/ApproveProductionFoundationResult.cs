using System;

namespace ProjectVictory.Application.Production;

public sealed record ApproveProductionFoundationResult(
    Guid ApprovalId,
    bool Approved);