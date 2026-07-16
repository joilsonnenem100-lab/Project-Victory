using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Character;

public sealed record CreateEvolutionApplicationRequestCommand(
    Guid CharacterId,
    IReadOnlyCollection<EvolutionEffect> Effects);