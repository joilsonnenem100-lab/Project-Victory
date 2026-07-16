using System;

namespace ProjectVictory.Application.Character;

public sealed record ConfirmEvolutionSynchronizationResult(
    Guid CharacterId,
    bool Confirmed);