using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ConfigureHuntContentCommand(
    Guid HuntAreaId,
    Guid EncounterId);