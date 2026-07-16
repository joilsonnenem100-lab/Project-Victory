using System;

namespace ProjectVictory.Application.Progression;

public sealed record CompleteEvolutionCycleCommand(
    Guid CharacterId,
    bool CharacterResultReceived);