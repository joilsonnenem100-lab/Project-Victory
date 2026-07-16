using System;

namespace ProjectVictory.Domain.Progression;

public sealed record EvolutionResult(
    Guid CharacterId,
    bool EvolutionAvailable);