using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessEvolutionResult(
    Guid CharacterId,
    bool EvolutionAvailable);