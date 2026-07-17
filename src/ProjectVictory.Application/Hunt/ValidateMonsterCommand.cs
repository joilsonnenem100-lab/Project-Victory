using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateMonsterCommand(
    Guid WorldId,
    Guid HuntAreaId,
    Guid EncounterId,
    Guid MonsterId,
    bool CombatAvailable);