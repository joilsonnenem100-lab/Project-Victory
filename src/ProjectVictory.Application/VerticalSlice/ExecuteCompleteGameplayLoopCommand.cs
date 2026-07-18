using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ExecuteCompleteGameplayLoopCommand(
    Guid CharacterId,
    Guid SessionId);