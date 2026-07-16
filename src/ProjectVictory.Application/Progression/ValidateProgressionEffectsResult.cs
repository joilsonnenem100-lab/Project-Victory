using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateProgressionEffectsResult(
    Guid CharacterId,
    bool Valid);