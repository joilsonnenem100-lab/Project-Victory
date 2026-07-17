using System;

namespace ProjectVictory.Application.Combat;

public sealed class CreateGameplayActionHandler
{
    public CreateGameplayActionResult Handle(
        CreateGameplayActionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CombatContextId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CombatContextId is required.");
        }

        if (command.ActionId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "ActionId is required.");
        }

        return new CreateGameplayActionResult(
            command.CombatContextId,
            command.ActionId,
            true);
    }
}