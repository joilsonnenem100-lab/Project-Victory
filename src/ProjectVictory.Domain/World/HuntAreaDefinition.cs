using System;

namespace ProjectVictory.Domain.World;

public sealed record HuntAreaDefinition(
    Guid Id,
    Guid LocationId,
    string Name);