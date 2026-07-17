using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateWorldHuntContextResult(
    Guid HuntId,
    bool Valid);