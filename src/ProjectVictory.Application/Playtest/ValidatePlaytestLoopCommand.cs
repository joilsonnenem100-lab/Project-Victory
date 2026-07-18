using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidatePlaytestLoopCommand(
    Guid FeedbackId,
    bool IssueIdentified,
    bool CorrectionApplied,
    bool ValidationCompleted);