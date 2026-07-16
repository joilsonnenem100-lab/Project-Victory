using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class GenerateCompositeEvolutionResultHandler
{
    public CompositeEvolutionResult Handle(
        GenerateCompositeEvolutionResultCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new CompositeEvolutionResult(
            command.CharacterId,
            command.Effects,
            true);
    }
}