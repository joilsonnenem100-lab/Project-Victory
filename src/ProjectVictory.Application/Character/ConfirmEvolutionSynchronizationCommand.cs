using System;

namespace ProjectVictory.Application.Character;

public sealed record ConfirmEvolutionSynchronizationCommand(
    Guid CharacterId,
    bool Compatible);