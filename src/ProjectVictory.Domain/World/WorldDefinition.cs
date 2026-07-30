using System;

namespace ProjectVictory.Domain.World;

public sealed record WorldDefinition(
    Guid Id,
    string Name,
    string Description,
    WorldDefinitionStatus Status);
