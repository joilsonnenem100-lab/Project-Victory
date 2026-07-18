using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateGameplayLoopStabilityCommand(
    Guid CharacterId,
    int Executions,
    bool StateConsistent);