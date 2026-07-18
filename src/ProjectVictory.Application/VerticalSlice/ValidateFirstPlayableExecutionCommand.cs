using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateFirstPlayableExecutionCommand(
    Guid CharacterId,
    bool EnvironmentInitialized,
    bool GameplayExecuted,
    bool StatePersisted);