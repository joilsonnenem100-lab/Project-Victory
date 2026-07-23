using System;

namespace ProjectVictory.Domain.Runtime;

public sealed record RuntimeExecutionContext(
    Guid RuntimeId,
    Guid CharacterId,
    bool Initialized,
    CharacterRuntimeState? CharacterState = null);
