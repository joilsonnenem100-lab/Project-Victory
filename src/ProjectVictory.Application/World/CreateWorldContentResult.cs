using System;

namespace ProjectVictory.Application.World;

public sealed record CreateWorldContentResult(
    Guid WorldId,
    Guid LocationId,
    bool Created);