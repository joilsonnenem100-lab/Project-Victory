using System;

namespace ProjectVictory.Application.Combat;

public sealed record CombatRequest(
    Guid CombatContextId,
    Guid AttackerId,
    Guid MonsterId);