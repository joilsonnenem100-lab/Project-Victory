using System;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessEvolutionEffectsResult(
    Guid CharacterId,
    bool Processed);