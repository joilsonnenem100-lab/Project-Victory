using System;

namespace ProjectVictory.Application.Hunt;

public sealed record PrepareHuntContentResult(
    Guid HuntContentId,
    bool Prepared);