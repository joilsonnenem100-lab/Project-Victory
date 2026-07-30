using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record GameplayActionExecutionCommand(
    Guid ActionId,
    Guid CharacterId);
