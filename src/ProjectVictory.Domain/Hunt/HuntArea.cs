using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record HuntArea(
    Guid HuntAreaId,
    Guid LocationId,
    string Name);