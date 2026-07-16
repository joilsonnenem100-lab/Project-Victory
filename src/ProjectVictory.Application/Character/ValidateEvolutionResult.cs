using System;

namespace ProjectVictory.Application.Character;

public sealed record ValidateEvolutionResult(
    Guid CharacterId,
    bool Accepted);