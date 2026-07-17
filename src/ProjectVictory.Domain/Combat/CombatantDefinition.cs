using System;

namespace ProjectVictory.Domain.Combat;

public sealed record CombatantDefinition(
    Guid CombatantId,
    string Name,
    CombatantType Type);