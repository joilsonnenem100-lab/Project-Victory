using System;

namespace ProjectVictory.Application.Character;

public sealed record ApplyEvolutionResult(
    Guid CharacterId,
    bool Applied);