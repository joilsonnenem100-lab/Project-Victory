using System;

namespace ProjectVictory.Application.Combat;

public sealed record ResolveCombatCommand(
    Guid CombatContextId,
    Guid AttackerId,
    Guid MonsterId);
