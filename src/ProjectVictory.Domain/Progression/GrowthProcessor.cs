namespace ProjectVictory.Domain.Progression;

public sealed class GrowthProcessor
{
    public GrowthResult Process(
        ExperienceResult experienceResult,
        ExperienceTable table)
    {
        ArgumentNullException.ThrowIfNull(experienceResult);
        ArgumentNullException.ThrowIfNull(table);


        // PV-PROG-011: interpreta a Experience pela tabela recebida (ADR-025).
        var level =
            new Level(
                table.GetLevel(
                    experienceResult.ExperienceAmount));


        return new GrowthResult(
            experienceResult.CharacterId,
            experienceResult.ExperienceAmount,
            level.Value,
            experienceResult.Processed);
    }
}