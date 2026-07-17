using System;

namespace ProjectVictory.Application.PlayerExperience;

public sealed record ValidateFinalPlayerExperienceResult(
    Guid SessionId,
    bool Completed);