using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidateSessionExecutionStabilityResult(
    Guid SessionId,
    bool Valid);