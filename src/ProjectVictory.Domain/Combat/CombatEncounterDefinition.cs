using System;

namespace ProjectVictory.Domain.Combat;

public sealed record CombatEncounterDefinition(
    Guid EncounterId,
    Guid HuntAreaId);