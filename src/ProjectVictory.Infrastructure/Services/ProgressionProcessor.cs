using ProjectVictory.Application.Progression;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.Infrastructure.Services;

public sealed class ProgressionProcessor : IProgressionProcessor
{
    public ProgressionResult Process(
        ProgressionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);


        var experience =
            request.ExperienceGain;


        var level =
            CalculateLevel(
                experience);



        return new ProgressionResult(
            experience,
            level);
    }



    // PV-PROG-011: o Level é interpretado pela Experience Table (ADR-025), gerada pela curva
    // do DEC-20261106-10, sem teto fixo em 5 (teto = Level 500 por Ascension, ADR-026).
    private static int CalculateLevel(
        int experience)
    {
        return ExperienceTable.Standard.GetLevel(
            experience);
    }
}