using System;

namespace ProjectVictory.Application.Playtest;

public sealed class ValidatePlaytestLoopHandler
{
    public ValidatePlaytestLoopResult Handle(
        ValidatePlaytestLoopCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var completed =
            command.FeedbackId != Guid.Empty &&
            command.IssueIdentified &&
            command.CorrectionApplied &&
            command.ValidationCompleted;

        return new ValidatePlaytestLoopResult(
            command.FeedbackId,
            completed);
    }
}