using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateCharacterEvolutionResultResult(
    Guid CharacterId,
    bool Created);