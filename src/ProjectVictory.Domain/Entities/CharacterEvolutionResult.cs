using System;

namespace ProjectVictory.Domain.Entities;

public sealed record CharacterEvolutionResult(
    Guid CharacterId,
    bool Applied);