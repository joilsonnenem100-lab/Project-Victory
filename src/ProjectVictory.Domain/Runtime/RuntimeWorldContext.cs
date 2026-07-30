namespace ProjectVictory.Domain.Runtime;

public sealed record RuntimeWorldContext(
    Guid WorldId,
    Guid RegionId,
    Guid LocationId);
