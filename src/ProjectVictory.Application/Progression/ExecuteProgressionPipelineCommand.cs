using System;

namespace ProjectVictory.Application.Progression;

public sealed record ExecuteProgressionPipelineCommand(
    Guid CharacterId,
    int ExperienceAmount);