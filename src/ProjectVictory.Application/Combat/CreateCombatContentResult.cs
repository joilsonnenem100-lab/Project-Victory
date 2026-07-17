using System;

namespace ProjectVictory.Application.Combat;

public sealed record CreateCombatContentResult(
    Guid EncounterId,
    Guid CombatContentId,
    bool Created);