using System;

namespace ProjectVictory.Application.Combat;

public sealed class ExpandCombatResultHandler
{
    public ExpandedCombatResult Handle(
        ExpandCombatResultCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return new ExpandedCombatResult(
            command.Result.WinnerId,
            command.Result.Completed,
            command.CombatContextId);
    }
}