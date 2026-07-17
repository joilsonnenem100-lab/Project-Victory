using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionMilestoneCommand(
    Guid CharacterId,
    bool ProgressionProcessed,
    bool CharacterUpdated);