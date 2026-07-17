using System;

namespace ProjectVictory.Domain.Progression;

public sealed class ProgressionMilestone
{
    public Guid Id { get; }

    public string Name { get; }

    public int RequiredExperience { get; }


    public ProgressionMilestone(
        Guid id,
        string name,
        int requiredExperience)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Milestone id is required.",
                nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (requiredExperience < 0)
        {
            throw new ArgumentException(
                "Required experience cannot be negative.",
                nameof(requiredExperience));
        }

        Id = id;
        Name = name;
        RequiredExperience = requiredExperience;
    }
}