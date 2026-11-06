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



    // PV-PROG-011/012: o Level é interpretado pela Experience Table (ADR-025), a curva do
    // DEC-20261106-10, sem teto de Level (DEC-20261106-19; o antigo teto de 500 foi superado).
    private static int CalculateLevel(
        int experience)
    {
        return ExperienceTable.ForAscension(ExperienceTable.FirstAscension).GetLevel(
            experience);
    }
}