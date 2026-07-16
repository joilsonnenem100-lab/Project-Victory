using System;

namespace ProjectVictory.Domain.Entities;

public sealed record EvolutionApplicationValidationResult(
    Guid CharacterId,
    bool Accepted);