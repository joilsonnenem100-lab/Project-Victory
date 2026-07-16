using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Application.Character;

public sealed class ValidateCompositeEvolutionHandler
{
    public ValidateCompositeEvolutionResult Handle(
        ValidateCompositeEvolutionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var request = new EvolutionApplicationRequest(
            command.CharacterId,
            command.Effects);

        var accepted =
            request.Effects.Count > 0;

        return new ValidateCompositeEvolutionResult(
            request.CharacterId,
            accepted);
    }
}