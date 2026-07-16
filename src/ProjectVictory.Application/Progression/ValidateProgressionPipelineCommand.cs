using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionPipelineCommand(
    Guid CharacterId,
    bool CharacterApplied);