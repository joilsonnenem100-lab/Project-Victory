using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Domain.Entities;

public sealed record EvolutionApplicationRequest(
    Guid CharacterId,
    IReadOnlyCollection<EvolutionEffect> Effects);