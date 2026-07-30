using System;

namespace ProjectVictory.Application.Combat;

public sealed record ResolveCombatResult(
    Guid CombatContextId,
    Guid WinnerId,
    bool Completed);
