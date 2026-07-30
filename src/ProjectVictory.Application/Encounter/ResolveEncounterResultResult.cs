using System;
using ProjectVictory.Domain.Combat;

namespace ProjectVictory.Application.Encounter;

public sealed class ResolveEncounterResultResult
{
    public Guid EncounterId { get; }

    public CombatOutcome Result { get; }

    public string Status { get; }

    public ResolveEncounterResultResult(
        Guid encounterId,
        CombatOutcome result,
        string status)
    {
        EncounterId = encounterId;
        Result = result;
        Status = status;
    }
}
