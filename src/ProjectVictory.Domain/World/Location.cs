using System;

namespace ProjectVictory.Domain.World;

public sealed record Location(
    Guid LocationId,
    Guid WorldId,
    string Name);