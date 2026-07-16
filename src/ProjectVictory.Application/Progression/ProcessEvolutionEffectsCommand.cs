using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed record ProcessEvolutionEffectsCommand(
    Guid CharacterId,
    IReadOnlyCollection<EvolutionEffect> Effects);