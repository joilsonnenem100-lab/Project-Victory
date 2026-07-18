using System;

namespace ProjectVictory.Application.Playtest;

public sealed record AnalyzeTechnicalIssueCommand(
    Guid IssueId,
    string Description);