using System;

namespace ProjectVictory.Application.Gameplay;

public sealed record GameplayActionExecutionResult(
    Guid ActionId,
    string GameplaySystem);
