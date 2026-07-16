using System;

namespace ProjectVictory.Application.Progression;

public sealed record ReceiveCharacterEvolutionResultCommand(
    Guid CharacterId,
    bool Applied);