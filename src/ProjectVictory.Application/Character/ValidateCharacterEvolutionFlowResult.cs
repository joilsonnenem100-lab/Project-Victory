using System;

namespace ProjectVictory.Application.Character;

public sealed record ValidateCharacterEvolutionFlowResult(
    Guid CharacterId,
    bool Validated);