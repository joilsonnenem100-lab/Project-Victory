using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ProcessExperienceHandler
{
    private readonly ExperienceProcessor _processor;

    public ProcessExperienceHandler()
    {
        _processor = new ExperienceProcessor();
    }

    public ProcessExperienceResult Handle(
        ProcessExperienceCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var context = new ProgressionContext(
            Guid.NewGuid(),
            command.CharacterId,
            command.ExperienceAmount);

        var experience = new Experience(
            command.ExperienceAmount);

        var result = _processor.Process(
            context,
            experience);

        return new ProcessExperienceResult(
            result.CharacterId,
            result.ExperienceAmount,
            result.Processed);
    }
}