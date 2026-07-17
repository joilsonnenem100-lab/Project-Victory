using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateCharacterGrowthCommand(
    Guid CharacterId,
    int ExperienceAmount,
    int Level);