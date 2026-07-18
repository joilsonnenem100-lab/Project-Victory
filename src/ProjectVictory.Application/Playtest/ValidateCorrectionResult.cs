using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateCorrectionResult(
    Guid IssueId,
    bool Validated);