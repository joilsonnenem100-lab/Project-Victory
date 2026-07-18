using System;

namespace ProjectVictory.Application.Playtest;

public sealed class ValidateImprovementFinalHandler
{
    public ValidateImprovementFinalResult Handle(
        ValidateImprovementFinalCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var completed =
            command.RequestId != Guid.Empty &&
            command.ImprovementValidated &&
            command.VerticalSlicePreserved;

        return new ValidateImprovementFinalResult(
            command.RequestId,
            completed);
    }
}