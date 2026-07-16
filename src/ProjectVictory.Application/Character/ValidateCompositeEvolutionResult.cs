using System;

namespace ProjectVictory.Application.Character;

public sealed record ValidateCompositeEvolutionResult(
    Guid CharacterId,
    bool Accepted);