using ProjectVictory.Application.Progression;

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



    private static int CalculateLevel(
        int experience)
    {
        if (experience < 100)
        {
            return 1;
        }


        if (experience < 300)
        {
            return 2;
        }


        if (experience < 600)
        {
            return 3;
        }


        if (experience < 1000)
        {
            return 4;
        }


        return 5;
    }
}