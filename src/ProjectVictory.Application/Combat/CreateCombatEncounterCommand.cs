using System;

namespace ProjectVictory.Application.Combat;

public sealed record CreateCombatEncounterCommand(
    Guid EncounterId,
    Guid HuntAreaId);