using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ConfigureHuntContentResult(
    Guid HuntAreaId,
    bool Configured);