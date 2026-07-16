using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ExecuteProgressionPipelineHandler
{
    private readonly IProgressionProcessor _progressionProcessor;

    public ExecuteProgressionPipelineHandler(
        IProgressionProcessor progressionProcessor)
    {
        _progressionProcessor = progressionProcessor;
    }

    public ExecuteProgressionPipelineResult Handle(
        ExecuteProgressionPipelineCommand command)
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

        return new ExecuteProgressionPipelineResult(
            context.CharacterId,
            true,
            result.Experience,
            result.Level);
    }
}