using System;

namespace ProjectVictory.Application.Playtest;

public sealed class ValidateCorrectionHandler
{
    public ValidateCorrectionResult Handle(
        ValidateCorrectionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validated =
            command.IssueId != Guid.Empty &&
            command.CorrectionApplied &&
            command.BehaviorPreserved;

        return new ValidateCorrectionResult(
            command.IssueId,
            validated);
    }
}