using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class GenerateExperienceResultHandler
{
    private readonly ExperienceProcessor _processor;

    public GenerateExperienceResultHandler()
    {
        _processor = new ExperienceProcessor();
    }

    public ExperienceResult Handle(
        GenerateExperienceResultCommand command)
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

        return _processor.Process(
            context,
            experience);
    }
}