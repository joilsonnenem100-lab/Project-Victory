using System;
using System.Collections.Generic;

namespace ProjectVictory.Application.World;

public sealed class ResolveLocationContentHandler
{
    public ResolveLocationContentResult Handle(
        ResolveLocationContentCommand command)
    {
        if (command.LocationId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Location not found.");
        }

        return new ResolveLocationContentResult(
            command.LocationId,
            new List<string>
            {
                "Exploration"
            });
    }
}
