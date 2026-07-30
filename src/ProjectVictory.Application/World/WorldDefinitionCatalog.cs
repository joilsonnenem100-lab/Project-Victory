using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ProjectVictory.Domain.World;

namespace ProjectVictory.Application.World;

public sealed class WorldDefinitionCatalog : IWorldDefinitionCatalog
{
    private readonly IReadOnlyDictionary<Guid, WorldDefinition> _worlds;
    private readonly IReadOnlyDictionary<Guid, RegionDefinition> _regions;
    private readonly IReadOnlyDictionary<Guid, LocationDefinition> _locations;

    public WorldDefinitionCatalog(
        IEnumerable<WorldDefinition> worlds,
        IEnumerable<RegionDefinition> regions,
        IEnumerable<LocationDefinition> locations)
    {
        _worlds = new ReadOnlyDictionary<Guid, WorldDefinition>(
            worlds.ToDictionary(x => x.Id));

        _regions = new ReadOnlyDictionary<Guid, RegionDefinition>(
            regions.ToDictionary(x => x.Id));

        _locations = new ReadOnlyDictionary<Guid, LocationDefinition>(
            locations.ToDictionary(x => x.Id));
    }

    public WorldDefinition? GetWorld(Guid worldId)
    {
        return _worlds.TryGetValue(worldId, out var world)
            ? world
            : null;
    }

    public RegionDefinition? GetRegion(Guid worldId, Guid regionId)
    {
        return _regions.TryGetValue(regionId, out var region)
            && region.WorldId == worldId
            ? region
            : null;
    }

    public LocationDefinition? GetLocation(Guid regionId, Guid locationId)
    {
        return _locations.TryGetValue(locationId, out var location)
            && location.RegionId == regionId
            ? location
            : null;
    }
}
