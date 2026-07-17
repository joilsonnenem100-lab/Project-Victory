using System;

namespace ProjectVictory.Application.Character;

public sealed record CreatePersistentCharacterStateResult(
    Guid CharacterId,
    bool Created);