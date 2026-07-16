using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class CreateProgressionStateHandler
{
    public CreateProgressionStateResult Handle(
        CreateProgressionStateCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.ExperienceAmount < 0)
        {
            throw new InvalidOperationException(
                "Experience amount is invalid.");
        }

        var state = new ProgressionState(
            command.CharacterId,
            command.ExperienceAmount,
            command.Completed);

        return new CreateProgressionStateResult(
            state.CharacterId,
            true);
    }
}