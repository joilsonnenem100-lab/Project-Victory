using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ValidateMonsterResult(
    Guid MonsterId,
    bool Valid);