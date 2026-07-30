using System;

namespace ProjectVictory.Application.World;

public sealed record EvaluateContentAvailabilityCommand(
    Guid ContentId);
