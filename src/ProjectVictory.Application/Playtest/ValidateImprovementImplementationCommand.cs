using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateImprovementImplementationCommand(
    Guid RequestId,
    bool ArchitectureApproved,
    bool ImplementationCompleted);