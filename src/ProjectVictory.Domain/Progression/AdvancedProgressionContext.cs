using System;

namespace ProjectVictory.Domain.Progression;

public sealed record AdvancedProgressionContext(
    Guid CharacterId,
    int ExperienceAmount,
    bool Enabled);