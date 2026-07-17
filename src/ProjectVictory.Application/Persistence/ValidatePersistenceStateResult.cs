using System;

namespace ProjectVictory.Application.Persistence;

public sealed record ValidatePersistenceStateResult(
    Guid StateId,
    bool Valid);