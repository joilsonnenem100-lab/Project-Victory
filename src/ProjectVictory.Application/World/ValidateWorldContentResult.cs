using System;

namespace ProjectVictory.Application.World;

public sealed record ValidateWorldContentResult(
    Guid WorldId,
    bool Valid);