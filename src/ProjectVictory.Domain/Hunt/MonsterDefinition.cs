using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record MonsterDefinition(
    Guid Id,
    string Name,
    Guid EncounterId);