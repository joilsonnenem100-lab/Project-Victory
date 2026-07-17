using System;

namespace ProjectVictory.Persistence.Character;

public sealed record PersistentCharacterState(
    Guid Id,
    Guid AccountId,
    string Name,
    int Level,
    int Experience);