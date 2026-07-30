using System;

namespace ProjectVictory.Application.Encounter;

public sealed class BindEncounterToRuntimeHandler
{
    public BindEncounterToRuntimeResult Handle(BindEncounterToRuntimeCommand command)
    {
        return new BindEncounterToRuntimeResult(
            command.RuntimeId,
            command.EncounterId);
    }
}
