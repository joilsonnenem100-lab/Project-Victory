using System;

namespace ProjectVictory.Application.World;

public sealed record CreateLocationCommand(
    Guid LocationId,
    Guid WorldId,
    string Name);