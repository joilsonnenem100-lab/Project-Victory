using System;

namespace ProjectVictory.Application.Playtest;

public sealed record AnalyzeTechnicalIssueResult(
    Guid IssueId,
    bool Analyzed);