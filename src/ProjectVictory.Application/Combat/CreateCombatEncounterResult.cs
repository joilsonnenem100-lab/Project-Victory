using System;

namespace ProjectVictory.Application.Combat;

public sealed record CreateCombatEncounterResult(
    Guid EncounterId,
    bool Created);