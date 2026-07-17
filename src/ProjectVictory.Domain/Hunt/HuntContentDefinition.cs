using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record HuntContentDefinition(
    Guid HuntAreaId,
    Guid EncounterId);