using System;

namespace ProjectVictory.Application.Progression;

public sealed record ValidateAdvancedProgressionResult(
    Guid CharacterId,
    bool Valid);