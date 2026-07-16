using System;

namespace ProjectVictory.Application.Progression;

public sealed record CreateProgressionStateCommand(
    Guid CharacterId,
    int ExperienceAmount,
    bool Completed);