using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateCharacterEvolutionResultCommand(
    Guid CharacterId,
    bool Applied);