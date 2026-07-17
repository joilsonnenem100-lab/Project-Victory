using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record SelectHuntCommand(
    Guid SessionId,
    Guid HuntId);