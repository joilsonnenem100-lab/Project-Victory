using System;

namespace ProjectVictory.Domain.World;

public sealed record ContentAvailabilityRule(
    Guid Id,
    Guid ContentId,
    ContentAvailabilityStatus Status);
