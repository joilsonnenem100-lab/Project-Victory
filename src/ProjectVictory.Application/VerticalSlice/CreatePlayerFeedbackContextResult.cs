using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record CreatePlayerFeedbackContextResult(
    Guid CharacterId,
    bool Created);