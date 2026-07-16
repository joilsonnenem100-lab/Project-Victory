using System;

namespace ProjectVictory.Application.Character;

public sealed record ApplyCompositeEvolutionResult(
    Guid CharacterId,
    bool Applied);