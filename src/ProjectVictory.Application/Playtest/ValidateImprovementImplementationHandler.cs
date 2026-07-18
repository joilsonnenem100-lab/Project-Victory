using System;

namespace ProjectVictory.Application.Playtest;

public sealed class ValidateImprovementImplementationHandler
{
    public ValidateImprovementImplementationResult Handle(
        ValidateImprovementImplementationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var approved =
            command.RequestId != Guid.Empty &&
            command.ArchitectureApproved &&
            command.ImplementationCompleted;

        return new ValidateImprovementImplementationResult(
            command.RequestId,
            approved);
    }
}