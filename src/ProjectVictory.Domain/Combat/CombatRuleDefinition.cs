using System;

namespace ProjectVictory.Domain.Combat;

public sealed record CombatRuleDefinition(
    Guid RuleId,
    string Name);