using System;

namespace ProjectVictory.Application.Playtest;

public sealed class ValidateRegressionHandler
{
    public ValidateRegressionResult Handle(
        ValidateRegressionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var passed =
            command.ValidationId != Guid.Empty &&
            command.PreviousBehaviorPreserved &&
            command.ExistingFlowValid;

        return new ValidateRegressionResult(
            command.ValidationId,
            passed);
    }
}