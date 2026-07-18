using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateImprovementFinalCommand(
    Guid RequestId,
    bool ImprovementValidated,
    bool VerticalSlicePreserved);