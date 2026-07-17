using System;

namespace ProjectVictory.Application.PlayerExperience;

public sealed record ValidatePlayerExperienceFlowResult(
    Guid CharacterId,
    bool Valid);