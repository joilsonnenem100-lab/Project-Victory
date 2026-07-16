using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateEvolutionApplicationRequestResult(
    Guid CharacterId,
    bool Created);