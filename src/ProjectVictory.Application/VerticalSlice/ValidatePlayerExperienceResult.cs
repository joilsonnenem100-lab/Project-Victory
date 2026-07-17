using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidatePlayerExperienceResult(
    Guid CharacterId,
    bool Validated,
    GameplayFeedbackResult Feedback);