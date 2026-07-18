using System;

namespace ProjectVictory.Application.Session;

public sealed record ValidateSessionFinalizationCommand(
    Guid SessionId,
    bool FinalStateValid,
    bool PersistenceValidated);