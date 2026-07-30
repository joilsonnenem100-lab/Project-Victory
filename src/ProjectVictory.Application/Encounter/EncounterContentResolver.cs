using System;
using ProjectVictory.Domain.Encounter;

namespace ProjectVictory.Application.Encounter;

public sealed class EncounterContentResolver
{
    public ResolveEncounterFromContentActionResult Handle(
        ResolveEncounterFromContentActionCommand command)
    {
        if (command.LocationId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Location is required.");
        }

        if (command.ActionId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Action is required.");
        }

        var encounter = new ProjectVictory.Domain.Encounter.Encounter(
            Guid.NewGuid());

        return new ResolveEncounterFromContentActionResult(
            encounter.Id,
            command.LocationId,
            command.ActionId,
            encounter.Status.ToString());
    }
}
