using System;

namespace ProjectVictory.Domain.Combat;

public sealed record CombatContentDefinition(
    Guid Id,
    Guid MonsterId,
    string Name);