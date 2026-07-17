using System;

namespace ProjectVictory.Application.PlayerExperience;

public sealed class ValidatePlayerExperienceFlowHandler
{
    public ValidatePlayerExperienceFlowResult Handle(
        ValidatePlayerExperienceFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var valid =
            command.SessionId != Guid.Empty &&
            command.CharacterId != Guid.Empty &&
            command.GameplayAvailable &&
            command.ExperienceStateCreated;

        return new ValidatePlayerExperienceFlowResult(
            command.CharacterId,
            valid);
    }
}