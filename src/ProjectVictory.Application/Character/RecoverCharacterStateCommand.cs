using System;

namespace ProjectVictory.Application.Character;

public sealed record RecoverCharacterStateCommand(
    Guid CharacterId);