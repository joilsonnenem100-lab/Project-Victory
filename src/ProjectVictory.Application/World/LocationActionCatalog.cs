using System;
using System.Collections.Generic;
using ProjectVictory.Domain.World;

namespace ProjectVictory.Application.World;

public sealed class LocationActionCatalog : ILocationActionCatalog
{
    private static readonly IReadOnlyList<LocationActionDefinition> Actions =
        new[]
        {
            new LocationActionDefinition(
                Guid.Parse("40000000-0000-0000-0000-000000000001"),
                Guid.Parse("30000000-0000-0000-0000-000000000001"),
                LocationActionType.Explore,
                LocationActionStatus.Active)
        };

    public IReadOnlyList<LocationActionDefinition> GetByLocation(Guid locationId)
    {
        return Actions;
    }
}
