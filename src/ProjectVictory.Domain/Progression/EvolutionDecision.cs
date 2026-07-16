using System;

namespace ProjectVictory.Domain.Progression;

public sealed record EvolutionDecision(
    Guid CharacterId,
    bool CanEvolve);