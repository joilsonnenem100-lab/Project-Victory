using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record HuntContentReference(
    Guid ContentId,
    Guid HuntAreaId,
    Guid EncounterId);