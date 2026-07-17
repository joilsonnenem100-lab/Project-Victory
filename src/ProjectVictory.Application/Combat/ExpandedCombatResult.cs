using System;

namespace ProjectVictory.Application.Combat;

public sealed record ExpandedCombatResult(
    Guid WinnerId,
    bool Completed,
    Guid CombatContextId);