using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Hunt;

public sealed record MonsterEncounterDefinition(
    Guid EncounterId,
    Guid HuntAreaId,
    IReadOnlyCollection<string> MonsterNames);