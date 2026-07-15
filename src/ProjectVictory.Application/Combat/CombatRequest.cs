using System;

namespace ProjectVictory.Application.Combat;

public sealed record CombatRequest(
    Guid AttackerId,
    Guid DefenderId);