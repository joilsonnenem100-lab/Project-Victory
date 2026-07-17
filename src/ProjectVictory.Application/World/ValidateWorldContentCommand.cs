using System;

namespace ProjectVictory.Application.World;

public sealed record ValidateWorldContentCommand(
    Guid WorldId,
    bool ContentLoaded,
    bool HuntAvailable);