using System;

namespace ProjectVictory.Application.Combat;

public sealed record ProcessCombatResultCommand(
    Guid CombatId);