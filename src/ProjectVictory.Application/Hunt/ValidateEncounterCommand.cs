using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateEncounterCommand(
    Guid WorldId,
    Guid HuntAreaId,
    Guid EncounterId,
    bool HuntAvailable,
    bool CombatAvailable);