using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class CalculateLevelHandler
{
    private readonly LevelCalculator _calculator;

    public CalculateLevelHandler()
    {
        _calculator = new LevelCalculator();
    }

    public LevelResult Handle(
        CalculateLevelCommand command)
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

        var experience = new global::ProjectVictory.Domain.Progression.Experience(
            context.ExperienceAmount);

        var level = _calculator.Calculate(
            experience);

        return new LevelResult(
            context.CharacterId,
            level.Value,
            true);
    }
}