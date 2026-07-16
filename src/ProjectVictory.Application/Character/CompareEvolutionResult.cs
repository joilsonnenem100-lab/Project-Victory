using System;

namespace ProjectVictory.Application.Character;

public sealed record CompareEvolutionResult(
    Guid CharacterId,
    bool Compatible);