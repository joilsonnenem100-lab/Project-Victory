using System;

namespace ProjectVictory.Application.Combat;

public sealed record CreateGameplayActionCommand(
    Guid CombatContextId,
    Guid ActionId);