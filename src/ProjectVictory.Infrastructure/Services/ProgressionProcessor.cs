using ProjectVictory.Application.Progression;

namespace ProjectVictory.Infrastructure.Services;

public sealed class ProgressionProcessor : IProgressionProcessor
{
    public ProgressionResult Process(
        ProgressionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new ProgressionResult(
            request.ExperienceGain,
            1);
    }
}