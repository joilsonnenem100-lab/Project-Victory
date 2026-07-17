using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Progression;

public sealed class ExperienceTable
{
    private readonly IReadOnlyDictionary<int, int> _levels;

    public ExperienceTable(
        IReadOnlyDictionary<int, int> levels)
    {
        ArgumentNullException.ThrowIfNull(levels);

        _levels = levels;
    }

    public int GetRequiredExperience(int level)
    {
        if (_levels.TryGetValue(level, out var experience))
        {
            return experience;
        }

        return 0;
    }
}