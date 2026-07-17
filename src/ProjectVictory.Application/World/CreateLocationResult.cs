using System;

namespace ProjectVictory.Application.World;

public sealed record CreateLocationResult(
    Guid LocationId,
    bool Created);