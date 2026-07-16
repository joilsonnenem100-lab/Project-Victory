using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessEvolutionCommand(
    Guid CharacterId,
    int ExperienceAmount);