using System;

namespace ProjectVictory.Application.Hunt;

public sealed record CreateMonsterEncounterCommand(
    Guid EncounterId,
    Guid MonsterId);