using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Progression;

public sealed class EvolutionEffectProcessor
{
    public ProcessedEvolutionEffects Process(
        EvolutionDecision decision,
        IReadOnlyCollection<EvolutionEffect> effects)
    {
        ArgumentNullException.ThrowIfNull(decision);
        ArgumentNullException.ThrowIfNull(effects);

        if (!decision.CanEvolve)
        {
            return new ProcessedEvolutionEffects(
                Array.Empty<EvolutionEffect>());
        }

        return new ProcessedEvolutionEffects(
            effects);
    }
}