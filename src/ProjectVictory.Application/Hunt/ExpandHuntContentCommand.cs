using System;

namespace ProjectVictory.Application.Hunt;

public sealed record ExpandHuntContentCommand(
    Guid HuntAreaId,
    Guid EncounterId);