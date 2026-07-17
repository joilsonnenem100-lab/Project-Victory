using System;

namespace ProjectVictory.Application.World;

public sealed record BindCharacterLocationResult(
    Guid CharacterId,
    bool Bound);