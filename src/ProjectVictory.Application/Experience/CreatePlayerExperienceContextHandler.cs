using ProjectVictory.Domain.PlayerExperience;

namespace ProjectVictory.Application.Experience;

public sealed class CreatePlayerExperienceContextHandler
{
    public CreatePlayerExperienceContextResult Handle(
        CreatePlayerExperienceContextCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var context = new PlayerExperienceContext(
            command.CharacterId,
            command.WorldId,
            command.HuntId);

        return new CreatePlayerExperienceContextResult(
            context.CharacterId,
            true);
    }
}