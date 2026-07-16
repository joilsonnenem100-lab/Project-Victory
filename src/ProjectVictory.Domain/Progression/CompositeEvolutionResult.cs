using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Progression;

public sealed record CompositeEvolutionResult(
    Guid CharacterId,
    IReadOnlyCollection<EvolutionEffect> Effects,
    bool Processed);