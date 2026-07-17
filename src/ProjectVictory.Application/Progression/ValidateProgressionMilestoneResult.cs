using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionMilestoneResult(
    Guid CharacterId,
    bool Valid);