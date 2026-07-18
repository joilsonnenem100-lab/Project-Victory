using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateCorrectionCommand(
    Guid IssueId,
    bool CorrectionApplied,
    bool BehaviorPreserved);