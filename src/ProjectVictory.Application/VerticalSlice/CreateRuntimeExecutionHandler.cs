using System;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreateRuntimeExecutionHandler
{
    public CreateRuntimeExecutionResult Handle(
        CreateRuntimeExecutionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var context = new RuntimeExecutionContext(
            command.RuntimeId,
            command.CharacterId,
            true);

        return new CreateRuntimeExecutionResult(
            context.RuntimeId,
            context.CharacterId,
            true);
    }
}