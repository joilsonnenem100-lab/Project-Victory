using System;

namespace ProjectVictory.Domain.Combat;

public sealed record CombatResolutionResult(
    Guid CombatId,
    bool Completed);