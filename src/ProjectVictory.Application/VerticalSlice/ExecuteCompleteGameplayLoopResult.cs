using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ExecuteCompleteGameplayLoopResult(
    Guid CharacterId,
    bool Completed);