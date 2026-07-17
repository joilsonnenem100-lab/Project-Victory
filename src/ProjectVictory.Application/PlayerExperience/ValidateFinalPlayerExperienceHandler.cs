using System;

namespace ProjectVictory.Application.PlayerExperience;

public sealed class ValidateFinalPlayerExperienceHandler
{
    public ValidateFinalPlayerExperienceResult Handle(
        ValidateFinalPlayerExperienceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var completed =
            command.SessionId != Guid.Empty &&
            command.CharacterId != Guid.Empty &&
            command.GameplayValidated &&
            command.ExperienceValidated &&
            command.PersistenceValidated;

        return new ValidateFinalPlayerExperienceResult(
            command.SessionId,
            completed);
    }
}