using System;

namespace ProjectVictory.Domain.World;

public sealed record RegionDefinition(
    Guid Id,
    Guid WorldId,
    string Name,
    string Description,
    RegionDefinitionStatus Status);
