using System;

namespace ProjectVictory.Application.Hunt;

public sealed record PrepareHuntContentCommand(
    Guid HuntContentId);