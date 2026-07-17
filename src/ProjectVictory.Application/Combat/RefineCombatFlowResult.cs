using System;

namespace ProjectVictory.Application.Combat;

public sealed record RefineCombatFlowResult(
    Guid CombatId,
    bool Validated);