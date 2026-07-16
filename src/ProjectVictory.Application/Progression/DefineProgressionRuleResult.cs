using System;

namespace ProjectVictory.Application.Progression;

public sealed record DefineProgressionRuleResult(
    Guid CharacterId,
    bool EvolutionAvailable);