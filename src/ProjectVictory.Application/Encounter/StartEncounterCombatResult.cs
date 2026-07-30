using System;

namespace ProjectVictory.Application.Encounter;

public sealed record StartEncounterCombatResult(
    Guid EncounterId,
    Guid CombatContextId);
