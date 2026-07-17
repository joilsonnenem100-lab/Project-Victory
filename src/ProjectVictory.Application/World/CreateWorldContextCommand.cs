using System;

namespace ProjectVictory.Application.World;

public sealed record CreateWorldContextCommand(
    Guid WorldId,
    string Name);