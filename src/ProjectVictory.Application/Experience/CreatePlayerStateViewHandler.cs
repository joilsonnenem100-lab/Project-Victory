using ProjectVictory.Domain.PlayerExperience;

namespace ProjectVictory.Application.Experience;

public sealed class CreatePlayerStateViewHandler
{
    public CreatePlayerStateViewResult Handle(
        CreatePlayerStateViewCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var view = new PlayerStateView(
            command.CharacterId,
            command.WorldId,
            command.HuntId,
            command.Level);

        return new CreatePlayerStateViewResult(
            view.CharacterId,
            true);
    }
}