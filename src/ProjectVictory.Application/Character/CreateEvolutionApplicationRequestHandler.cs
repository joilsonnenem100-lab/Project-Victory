using ProjectVictory.Domain.Entities;

namespace ProjectVictory.Application.Character;

public sealed class CreateEvolutionApplicationRequestHandler
{
    public CreateEvolutionApplicationRequestResult Handle(
        CreateEvolutionApplicationRequestCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var request = new EvolutionApplicationRequest(
            command.CharacterId,
            command.Effects);

        return new CreateEvolutionApplicationRequestResult(
            request.CharacterId,
            true);
    }
}