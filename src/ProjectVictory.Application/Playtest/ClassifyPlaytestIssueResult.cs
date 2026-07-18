using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ClassifyPlaytestIssueResult(
    Guid FeedbackId,
    bool Classified);