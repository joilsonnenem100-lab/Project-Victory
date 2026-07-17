using System;

namespace ProjectVictory.Application.Hunt;

public sealed record IntegrateGameplayContentCommand(
    Guid HuntAreaId,
    Guid EncounterId);