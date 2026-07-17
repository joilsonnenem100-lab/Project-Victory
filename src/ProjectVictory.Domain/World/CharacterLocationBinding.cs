using System;

namespace ProjectVictory.Domain.World;

public sealed record CharacterLocationBinding(
    Guid CharacterId,
    Guid LocationId);