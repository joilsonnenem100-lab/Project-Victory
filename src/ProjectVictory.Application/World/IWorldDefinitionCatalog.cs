using System;
using ProjectVictory.Domain.World;

namespace ProjectVictory.Application.World;

public interface IWorldDefinitionCatalog
{
    WorldDefinition? GetWorld(Guid worldId);

    RegionDefinition? GetRegion(Guid worldId, Guid regionId);

    LocationDefinition? GetLocation(Guid regionId, Guid locationId);
}
