using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateAdvancedProgressionCommand(
    Guid CharacterId,
    int ExperienceAmount);