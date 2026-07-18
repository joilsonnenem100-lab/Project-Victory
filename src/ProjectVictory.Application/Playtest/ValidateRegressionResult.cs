using System;

namespace ProjectVictory.Application.Playtest;

public sealed record ValidateRegressionResult(
    Guid ValidationId,
    bool Passed);