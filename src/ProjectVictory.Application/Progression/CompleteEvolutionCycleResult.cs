using System;

namespace ProjectVictory.Application.Progression;

public sealed record CompleteEvolutionCycleResult(
    Guid CharacterId,
    bool Completed);