using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateEvolutionSnapshotCommand(
    Guid CharacterId,
    int Level,
    bool HasEvolution);