using System;

namespace ProjectVictory.Domain.Entities;

public sealed record EvolutionValidationResult(
    Guid CharacterId,
    bool Accepted);