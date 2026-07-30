using System;

namespace ProjectVictory.Domain.World;

public sealed record LocationDefinition(
    Guid Id,
    Guid RegionId,
    string Name,
    string Description,
    LocationDefinitionStatus Status);
