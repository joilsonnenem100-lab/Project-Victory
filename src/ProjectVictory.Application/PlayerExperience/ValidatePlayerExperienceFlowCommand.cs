using System;

namespace ProjectVictory.Application.PlayerExperience;

public sealed record ValidatePlayerExperienceFlowCommand(
    Guid SessionId,
    Guid CharacterId,
    bool GameplayAvailable,
    bool ExperienceStateCreated);