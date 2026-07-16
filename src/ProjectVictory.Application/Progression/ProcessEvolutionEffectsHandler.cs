using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ProcessEvolutionEffectsHandler
{
    private readonly EvolutionEffectProcessor _processor;

    public ProcessEvolutionEffectsHandler()
    {
        _processor = new EvolutionEffectProcessor();
    }

    public ProcessEvolutionEffectsResult Handle(
        ProcessEvolutionEffectsCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var decision = new EvolutionDecision(
            command.CharacterId,
            command.Effects.Count > 0);

        var result = _processor.Process(
            decision,
            command.Effects);

        return new ProcessEvolutionEffectsResult(
            command.CharacterId,
            result.Effects.Count >= 0);
    }
}