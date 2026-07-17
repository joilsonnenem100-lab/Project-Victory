using System;

namespace ProjectVictory.Application.Experience;

public sealed record ExecutePlayerExperienceFlowCommand(
    Guid CharacterId,
    Guid WorldId);