using System;

namespace ProjectVictory.Application.Character;

public sealed record CreateEvolutionSnapshotResult(
    Guid CharacterId,
    bool Created);