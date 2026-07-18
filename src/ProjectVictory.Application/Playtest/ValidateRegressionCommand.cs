using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateRegressionCommand(
    Guid ValidationId,
    bool PreviousBehaviorPreserved,
    bool ExistingFlowValid);