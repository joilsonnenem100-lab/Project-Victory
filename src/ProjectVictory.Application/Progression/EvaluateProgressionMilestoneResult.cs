using System;

namespace ProjectVictory.Application.Progression;

public sealed record EvaluateProgressionMilestoneResult(
    Guid CharacterId,
    bool MilestoneReached);