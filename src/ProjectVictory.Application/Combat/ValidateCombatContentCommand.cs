using System;

namespace ProjectVictory.Application.Combat;

public sealed record ValidateCombatContentCommand(
    Guid WorldId,
    Guid EncounterId,
    Guid MonsterId,
    Guid CombatContentId,
    bool CombatExecuted,
    bool ResultGenerated);