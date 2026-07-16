using System;

namespace ProjectVictory.Application.Progression;

public sealed record ReceiveCharacterEvolutionResultResult(
    Guid CharacterId,
    bool Received);