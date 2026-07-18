using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidateSessionExecutionStabilityCommand(
    Guid SessionId,
    bool Active,
    bool StatePreserved);