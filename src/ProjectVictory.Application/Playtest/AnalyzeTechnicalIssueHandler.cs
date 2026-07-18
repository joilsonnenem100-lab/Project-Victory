using System;

namespace ProjectVictory.Application.Playtest;

public sealed class AnalyzeTechnicalIssueHandler
{
    public AnalyzeTechnicalIssueResult Handle(
        AnalyzeTechnicalIssueCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var analyzed =
            command.IssueId != Guid.Empty &&
            !string.IsNullOrWhiteSpace(command.Description);

        return new AnalyzeTechnicalIssueResult(
            command.IssueId,
            analyzed);
    }
}