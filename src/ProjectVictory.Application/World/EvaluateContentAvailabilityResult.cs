using System;

namespace ProjectVictory.Application.World;

public sealed record EvaluateContentAvailabilityResult(
    Guid ContentId,
    bool Available);
