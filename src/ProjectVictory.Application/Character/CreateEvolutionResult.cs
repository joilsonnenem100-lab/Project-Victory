using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateEvolutionResult(
    Guid CharacterId,
    bool Accepted);