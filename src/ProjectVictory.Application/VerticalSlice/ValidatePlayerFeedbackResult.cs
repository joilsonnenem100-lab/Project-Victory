using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidatePlayerFeedbackResult(
    Guid CharacterId,
    bool Valid);