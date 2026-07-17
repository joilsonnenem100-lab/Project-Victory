using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateCharacterGrowthCommand(
    Guid CharacterId,
    bool ProgressionProcessed,
    bool CharacterUpdated);