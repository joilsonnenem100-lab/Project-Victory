using System;

namespace ProjectVictory.Application.Architecture;

public sealed record ValidateArchitectureConsistencyResult(
    Guid ValidationId,
    bool Consistent);