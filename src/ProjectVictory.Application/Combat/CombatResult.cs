using System;

namespace ProjectVictory.Application.Combat;

public sealed record CombatResult(
    Guid WinnerId,
    bool Completed);