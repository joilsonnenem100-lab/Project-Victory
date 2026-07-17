using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CreateGameplayFeedbackCommand(
    Guid CharacterId,
    bool Success);