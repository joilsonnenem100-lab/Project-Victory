using System;

namespace ProjectVictory.Application.Character;

public sealed record ExecuteCharacterEvolutionFlowCommand(
    Guid CharacterId);