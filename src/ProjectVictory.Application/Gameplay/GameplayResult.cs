using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record GameplayResult(
    Guid SourceId,
    GameplayResultType Type);
