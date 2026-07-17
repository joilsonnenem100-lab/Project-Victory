using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateWorldHuntContextCommand(
    Guid WorldId,
    Guid LocationId,
    Guid HuntId);