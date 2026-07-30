using System;

namespace ProjectVictory.Application.World;

public sealed class WorldDefinitionStore : IWorldDefinitionStore
{
    public bool Exists(Guid worldId) => worldId != Guid.Empty;

    public bool ContainsRegion(Guid worldId, Guid regionId) =>
        Exists(worldId) && regionId != Guid.Empty;

    public bool ContainsLocation(Guid regionId, Guid locationId) =>
        regionId != Guid.Empty && locationId != Guid.Empty;
}
