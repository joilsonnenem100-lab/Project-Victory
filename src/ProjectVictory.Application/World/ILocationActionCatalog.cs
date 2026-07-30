using System;
using System.Collections.Generic;
using ProjectVictory.Domain.World;

namespace ProjectVictory.Application.World;

public interface ILocationActionCatalog
{
    IReadOnlyList<LocationActionDefinition> GetByLocation(Guid locationId);
}
