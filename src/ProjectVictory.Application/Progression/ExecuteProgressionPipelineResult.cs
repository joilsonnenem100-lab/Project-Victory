using System;

namespace ProjectVictory.Application.Progression;

public sealed record ExecuteProgressionPipelineResult(
    Guid CharacterId,
    bool Completed,
    int Experience,
    int Level);