using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateAdvancedProgressionCommand(
    Guid CharacterId,
    bool ProgressionProcessed,
    bool CharacterUpdated,
    bool PersistenceUpdated);