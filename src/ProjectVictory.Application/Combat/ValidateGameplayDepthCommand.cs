using System;

namespace ProjectVictory.Application.Combat;

public sealed record ValidateGameplayDepthCommand(
    Guid CombatContextId,
    bool CombatCompleted);