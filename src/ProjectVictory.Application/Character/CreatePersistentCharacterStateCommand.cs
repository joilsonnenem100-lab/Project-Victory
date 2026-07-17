using System;

namespace ProjectVictory.Application.Character;

public sealed record CreatePersistentCharacterStateCommand(
    Guid CharacterId);