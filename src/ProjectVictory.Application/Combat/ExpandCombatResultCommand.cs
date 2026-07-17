using System;

namespace ProjectVictory.Application.Combat;

public sealed record ExpandCombatResultCommand(
    Guid CombatContextId,
    CombatResult Result);