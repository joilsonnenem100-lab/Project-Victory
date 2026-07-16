using System;

namespace ProjectVictory.Application.Progression;

public sealed record ConfirmProgressionRuleCommand(
    Guid CharacterId,
    bool Validated);