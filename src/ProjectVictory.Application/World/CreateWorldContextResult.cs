using System;

namespace ProjectVictory.Application.World;

public sealed record CreateWorldContextResult(
    Guid WorldId,
    bool Created);