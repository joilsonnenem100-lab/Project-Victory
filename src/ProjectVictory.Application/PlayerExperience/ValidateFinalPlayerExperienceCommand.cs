using System;

namespace ProjectVictory.Application.PlayerExperience;

public sealed record ValidateFinalPlayerExperienceCommand(
    Guid SessionId,
    Guid CharacterId,
    bool GameplayValidated,
    bool ExperienceValidated,
    bool PersistenceValidated);