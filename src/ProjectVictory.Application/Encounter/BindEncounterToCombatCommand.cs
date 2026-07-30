using System;

namespace ProjectVictory.Application.Encounter;

public sealed record BindEncounterToCombatCommand(
    Guid EncounterId,
    Guid CombatContextId);
