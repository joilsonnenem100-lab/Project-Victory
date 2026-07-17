using System;

namespace ProjectVictory.Application.World;

public sealed record BindCharacterLocationCommand(
    Guid CharacterId,
    Guid LocationId);