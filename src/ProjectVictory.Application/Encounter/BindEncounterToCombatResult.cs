using System;

namespace ProjectVictory.Application.Encounter;

public sealed record BindEncounterToCombatResult(
    Guid EncounterId,
    Guid CombatContextId);
