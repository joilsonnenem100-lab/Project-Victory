using System;

namespace ProjectVictory.Application.Combat;

public sealed record CreateCombatContentCommand(
    Guid EncounterId,
    Guid MonsterId,
    Guid CombatContentId);