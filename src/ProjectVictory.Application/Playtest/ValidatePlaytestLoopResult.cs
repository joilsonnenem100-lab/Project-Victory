using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidatePlaytestLoopResult(
    Guid FeedbackId,
    bool Completed);