using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ProcessProgressionHandler
{
    private readonly IProgressionProcessor _progressionProcessor;

    public ProcessProgressionHandler(
        IProgressionProcessor progressionProcessor)
    {
        _progressionProcessor = progressionProcessor;
    }

    public ProcessProgressionResult Handle(
        ProcessProgressionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.ExperienceGain < 0)
        {
            throw new InvalidOperationException(
                "Experience gain is invalid.");
        }

        var context = new ProgressionContext(
            Guid.NewGuid(),
            command.CharacterId,
            command.ExperienceGain);

        var request = new ProgressionRequest(
            context.CharacterId,
            context.ExperienceAmount);

        _progressionProcessor.Process(
            request);

        return new ProcessProgressionResult(
            context.CharacterId,
            context.Id,
            true);
    }
}