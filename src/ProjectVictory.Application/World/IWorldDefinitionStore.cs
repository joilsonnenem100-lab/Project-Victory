using System;
using ProjectVictory.Domain.World;

namespace ProjectVictory.Application.World;

public interface IWorldDefinitionStore
{
    bool Exists(Guid worldId);
    bool ContainsRegion(Guid worldId, Guid regionId);
    bool ContainsLocation(Guid regionId, Guid locationId);
}
