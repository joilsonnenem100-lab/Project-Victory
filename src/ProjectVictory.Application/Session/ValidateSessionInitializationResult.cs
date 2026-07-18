using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidateSessionInitializationResult(
    Guid SessionId,
    bool Valid);