using System;

namespace ProjectVictory.Application.Combat;

public sealed record CreateGameplayActionResult(
    Guid CombatContextId,
    Guid ActionId,
    bool Created);