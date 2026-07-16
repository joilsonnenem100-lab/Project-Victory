using System;

namespace ProjectVictory.Application.Character;

public sealed record CompareEvolutionCommand(
    Guid CharacterId,
    int ProgressionLevel,
    int CharacterLevel);