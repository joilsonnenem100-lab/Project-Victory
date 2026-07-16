using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessProgressionCommand(
    Guid CharacterId,
    int ExperienceGain);