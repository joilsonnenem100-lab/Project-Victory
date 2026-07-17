using System;

namespace ProjectVictory.Application.Hunt;

public sealed record CreateMonsterEncounterResult(
    Guid EncounterId,
    Guid MonsterId,
    bool Created);