using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Application.Progression;

public sealed class ProcessExperienceRewardHandler
{
    private readonly ExperienceProcessor _experienceProcessor;
    private readonly GrowthProcessor _growthProcessor;

    public ProcessExperienceRewardHandler()
    {
        _experienceProcessor = new ExperienceProcessor();
        _growthProcessor = new GrowthProcessor();
    }

    public ProcessExperienceRewardResult Handle(
        ProcessExperienceRewardCommand command)
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

        var experience =
            new global::ProjectVictory.Domain.Progression.Experience(
                command.ExperienceAmount);

        var experienceResult =
            _experienceProcessor.Process(
                context,
                experience);

        // PV-PROG-011: tabela gerada pela curva do DEC-20261106-10 (antes: tabela de um Level só).
        var table = ExperienceTable.Standard;

        var growthResult =
            _growthProcessor.Process(
                experienceResult,
                table);

        return new ProcessExperienceRewardResult(
            growthResult.CharacterId,
            growthResult.ExperienceAmount,
            growthResult.Valid);
    }
}