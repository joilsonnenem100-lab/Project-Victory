using System;

namespace ProjectVictory.Application.Progression;

public sealed record GenerateEvolutionResultCommand(
    Guid CharacterId,
    int ExperienceAmount);