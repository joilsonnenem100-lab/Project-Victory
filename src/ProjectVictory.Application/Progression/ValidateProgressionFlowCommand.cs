using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionFlowCommand(
    Guid CharacterId,
    bool CharacterApplied);