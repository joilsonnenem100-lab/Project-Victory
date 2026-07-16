using System;

namespace ProjectVictory.Domain.Entities;

public sealed record EvolutionSnapshot(
    Guid CharacterId,
    int Level,
    bool HasEvolution);