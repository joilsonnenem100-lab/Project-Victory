using System;

namespace ProjectVictory.Domain.Playtest;

public sealed record TechnicalIssueAnalysis(
    Guid IssueId,
    string Description,
    string Analysis);