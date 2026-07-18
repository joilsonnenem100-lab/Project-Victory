using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidateSessionFinalizationResult(
    Guid SessionId,
    bool Approved);