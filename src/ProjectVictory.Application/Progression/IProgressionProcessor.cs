namespace ProjectVictory.Application.Progression;

public interface IProgressionProcessor
{
    ProgressionResult Process(
        ProgressionRequest request);
}