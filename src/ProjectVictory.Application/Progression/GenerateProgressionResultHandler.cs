using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class GenerateProgressionResultHandler
{
    private readonly IProgressionProcessor _progressionProcessor;

    public GenerateProgressionResultHandler(
        IProgressionProcessor progressionProcessor)
    {
        _progressionProcessor = progressionProcessor;
    }

    public ProgressionResult Handle(
        GenerateProgressionResultCommand command)
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

        var request = new ProgressionRequest(
            context.CharacterId,
            context.ExperienceAmount);

        var result = _progressionProcessor.Process(
            request);

        return result;
    }
}