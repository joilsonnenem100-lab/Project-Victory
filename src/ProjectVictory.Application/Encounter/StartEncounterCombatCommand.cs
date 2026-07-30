using System;

namespace ProjectVictory.Application.Encounter;

public sealed record StartEncounterCombatCommand(
    Guid EncounterId,
    Guid CharacterId);
