using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateImprovementImplementationResult(
    Guid RequestId,
    bool ApprovedForValidation);