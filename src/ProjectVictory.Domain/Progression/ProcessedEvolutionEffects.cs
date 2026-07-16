using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Progression;

public sealed class ProcessedEvolutionEffects
{
    public IReadOnlyCollection<EvolutionEffect> Effects { get; }

    public ProcessedEvolutionEffects(
        IReadOnlyCollection<EvolutionEffect> effects)
    {
        ArgumentNullException.ThrowIfNull(effects);

        Effects = effects;
    }
}