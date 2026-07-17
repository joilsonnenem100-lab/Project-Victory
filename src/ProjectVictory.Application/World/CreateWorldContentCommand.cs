using System;

namespace ProjectVictory.Application.World;

public sealed record CreateWorldContentCommand(
    Guid WorldId,
    Guid LocationId);