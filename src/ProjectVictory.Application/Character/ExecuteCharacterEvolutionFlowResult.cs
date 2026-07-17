using System;

namespace ProjectVictory.Application.Character;

public sealed record ExecuteCharacterEvolutionFlowResult(
    Guid CharacterId,
    bool Completed);