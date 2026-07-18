using System;
using ProjectVictory.Domain.Playtest;

namespace ProjectVictory.Application.Playtest;

public sealed class ClassifyPlaytestIssueHandler
{
    public ClassifyPlaytestIssueResult Handle(
        ClassifyPlaytestIssueCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var classified =
            command.FeedbackId != Guid.Empty &&
            command.Classification != PlaytestIssueClassification.Unknown;

        return new ClassifyPlaytestIssueResult(
            command.FeedbackId,
            classified);
    }
}