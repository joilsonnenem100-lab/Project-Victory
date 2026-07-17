using System;

namespace ProjectVictory.Application.Combat;

public sealed record RefineCombatFlowCommand(
    Guid CombatId);