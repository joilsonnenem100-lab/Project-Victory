using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateImprovementRequestResult(
    Guid RequestId,
    bool Validated);