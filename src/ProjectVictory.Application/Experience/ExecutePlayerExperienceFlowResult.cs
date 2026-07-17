using System;

namespace ProjectVictory.Application.Experience;

public sealed record ExecutePlayerExperienceFlowResult(
    Guid CharacterId,
    bool Completed);