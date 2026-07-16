using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionEffectsCommand(
    Guid CharacterId,
    int ExperienceAmount);