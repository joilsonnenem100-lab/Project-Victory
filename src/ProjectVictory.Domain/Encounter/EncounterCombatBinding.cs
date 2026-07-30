using System;

namespace ProjectVictory.Domain.Encounter;

public sealed record EncounterCombatBinding(
    Guid EncounterId,
    Guid CombatContextId);
