using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateEvolutionCommand(
    Guid CharacterId,
    Guid EvolutionResultId,
    bool EvolutionAvailable);