using System;

namespace ProjectVictory.Application.Character;

public sealed record RecoverCharacterStateResult(
    Guid CharacterId,
    bool Recovered);