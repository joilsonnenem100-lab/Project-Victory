using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidatePlayerSessionResult(
    Guid SessionId,
    bool Valid);