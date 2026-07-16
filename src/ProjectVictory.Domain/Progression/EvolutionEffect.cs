using System;

namespace ProjectVictory.Domain.Progression;

public sealed record EvolutionEffect(
    Guid CharacterId,
    string Type,
    int Value);