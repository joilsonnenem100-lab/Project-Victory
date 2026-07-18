using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateImprovementRequestCommand(
    Guid RequestId,
    bool RequestDescribed,
    bool ArchitecturalReviewRequired);