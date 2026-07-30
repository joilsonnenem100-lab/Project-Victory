using System;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Encounter;

public sealed class ResolveEncounterResultCommand
{
    public Guid EncounterId { get; }

    public CombatOutcome Result { get; }

    public ResolveEncounterResultCommand(
        Guid encounterId,
        CombatOutcome result)
    {
        EncounterId = encounterId;
        Result = result;
    }
}
