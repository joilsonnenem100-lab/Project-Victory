using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ExpandHuntContentResult(
    Guid HuntAreaId,
    bool Expanded);