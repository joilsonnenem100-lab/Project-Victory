using System;

namespace ProjectVictory.Application.Runtime;

public sealed record ChangeRuntimeLocationCommand(
    Guid RuntimeId,
    Guid WorldId,
    Guid RegionId,
    Guid LocationId);
