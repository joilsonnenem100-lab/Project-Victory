using System;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class EvaluateProgressionMilestoneHandler
{
    private readonly ProgressionMilestone _milestone;


    public EvaluateProgressionMilestoneHandler()
    {
        _milestone =
            new ProgressionMilestone(
                Guid.NewGuid(),
                "Initial Milestone",
                1000);
    }


    public EvaluateProgressionMilestoneResult Handle(
        EvaluateProgressionMilestoneCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reached =
            command.ExperienceAmount >=
            _milestone.RequiredExperience;

        return new EvaluateProgressionMilestoneResult(
            command.CharacterId,
            reached);
    }
}