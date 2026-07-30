using System;
using System.Collections.Generic;

namespace ProjectVictory.Application.World;

public sealed class ResolveLocationActionsHandler
{
    private readonly ILocationActionCatalog _catalog;

    public ResolveLocationActionsHandler(
        ILocationActionCatalog catalog)
    {
        _catalog = catalog;
    }

    public ResolveLocationActionsResult Handle(
        ResolveLocationActionsCommand command)
    {
        if (command.LocationId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Location not found.");
        }

        var availableActions = _catalog.GetByLocation(command.LocationId);

        return new ResolveLocationActionsResult(
            command.LocationId,
            availableActions);
    }

}
