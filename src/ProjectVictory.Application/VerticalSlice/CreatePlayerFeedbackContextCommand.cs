using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CreatePlayerFeedbackContextCommand(
    Guid CharacterId,
    bool Success,
    string Message);