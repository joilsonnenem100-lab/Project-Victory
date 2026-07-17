using System;

namespace ProjectVictory.Application.Combat;

public sealed class ValidateGameplayDepthHandler
{
    public ValidateGameplayDepthResult Handle(
        ValidateGameplayDepthCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CombatContextId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CombatContextId is required.");
        }

        return new ValidateGameplayDepthResult(
            command.CombatContextId,
            command.CombatCompleted);
    }
}