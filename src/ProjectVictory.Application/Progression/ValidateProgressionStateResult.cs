using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionStateResult(
    Guid CharacterId,
    bool Valid);