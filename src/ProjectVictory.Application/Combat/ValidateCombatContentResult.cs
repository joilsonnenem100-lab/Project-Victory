using System;

namespace ProjectVictory.Application.Combat;

public sealed record ValidateCombatContentResult(
    Guid CombatContentId,
    bool Valid);