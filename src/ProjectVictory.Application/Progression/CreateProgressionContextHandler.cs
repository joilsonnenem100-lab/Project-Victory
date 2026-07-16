using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class CreateProgressionContextHandler
{
    public CreateProgressionContextHandler()
    {
    }

    public Task<CreateProgressionContextResult> Handle(
        CreateProgressionContextCommand command,
        CancellationToken cancellationToken = default)
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

        var context = new ProgressionContext(
            Guid.NewGuid(),
            command.CharacterId,
            command.ExperienceAmount);

        return Task.FromResult(
            new CreateProgressionContextResult(
                context.Id,
                context.CharacterId));
    }
}