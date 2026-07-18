using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidatePlayerStateLoadingCommand(
    Guid CharacterId,
    bool CharacterLoaded);