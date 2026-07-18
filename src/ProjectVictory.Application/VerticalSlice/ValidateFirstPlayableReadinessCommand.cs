using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateFirstPlayableReadinessCommand(
    Guid CharacterId,
    bool GameplayReady,
    bool RuntimeReady,
    bool PersistenceReady);