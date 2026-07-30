using System;
using EncounterEntity = ProjectVictory.Domain.Encounter.Encounter;

namespace ProjectVictory.Application.Encounter;

public sealed class ResolveEncounterResultHandler
{
    public ResolveEncounterResultResult Handle(
        EncounterEntity encounter,
        ResolveEncounterResultCommand command)
    {
        if (encounter.Id != command.EncounterId)
        {
            throw new InvalidOperationException(
                "Encounter does not match result.");
        }

        encounter.Resolve();

        return new ResolveEncounterResultResult(
            encounter.Id,
            command.Result,
            encounter.Status.ToString());
    }
}
