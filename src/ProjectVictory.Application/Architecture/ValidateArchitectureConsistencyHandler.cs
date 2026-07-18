using System;

namespace ProjectVictory.Application.Architecture;

public sealed class ValidateArchitectureConsistencyHandler
{
    public ValidateArchitectureConsistencyResult Handle(
        ValidateArchitectureConsistencyCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var consistent =
            command.ValidationId != Guid.Empty &&
            command.BoundariesPreserved &&
            command.ContractsPreserved;

        return new ValidateArchitectureConsistencyResult(
            command.ValidationId,
            consistent);
    }
}