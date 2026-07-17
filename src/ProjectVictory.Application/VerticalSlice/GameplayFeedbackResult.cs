using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record GameplayFeedbackResult(
    Guid CharacterId,
    bool Success,
    string Message);