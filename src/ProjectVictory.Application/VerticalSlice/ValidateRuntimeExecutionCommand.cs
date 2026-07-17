using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateRuntimeExecutionCommand(
    Guid RuntimeId,
    Guid CharacterId,
    bool Initialized,
    bool GameplayExecuted,
    bool StateRecovered);