using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidatePlayerFeedbackCommand(
    Guid CharacterId,
    bool GameplayCompleted,
    bool FeedbackGenerated);