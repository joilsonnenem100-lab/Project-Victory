using System;

namespace ProjectVictory.Application.Combat;

public sealed record ValidateGameplayDepthResult(
    Guid CombatContextId,
    bool Validated);