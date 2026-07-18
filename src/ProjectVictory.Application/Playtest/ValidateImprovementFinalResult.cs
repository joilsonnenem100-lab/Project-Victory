using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateImprovementFinalResult(
    Guid RequestId,
    bool Completed);