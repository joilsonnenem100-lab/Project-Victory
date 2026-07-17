using System;

namespace ProjectVictory.Domain.World;

public sealed record WorldContext(
    Guid WorldId,
    string Name);