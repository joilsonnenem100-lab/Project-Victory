using System;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class ValidateGameplayLoopStabilityHandler
{
    public ValidateGameplayLoopStabilityResult Handle(
        ValidateGameplayLoopStabilityCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var stable =
            command.CharacterId != Guid.Empty &&
            command.Executions > 0 &&
            command.StateConsistent;

        return new ValidateGameplayLoopStabilityResult(
            command.CharacterId,
            stable);
    }
}