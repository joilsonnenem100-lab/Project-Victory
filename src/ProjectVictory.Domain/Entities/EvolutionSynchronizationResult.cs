using System;

namespace ProjectVictory.Domain.Entities;

public sealed record EvolutionSynchronizationResult(
    Guid CharacterId,
    bool Synchronized);