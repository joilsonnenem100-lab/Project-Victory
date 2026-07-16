using ProjectVictory.Application.Progression;

namespace ProjectVictory.Application.Reward;

public sealed class ForwardRewardToProgressionHandler
{
    private readonly IProgressionProcessor _progressionProcessor;

    public ForwardRewardToProgressionHandler(
        IProgressionProcessor progressionProcessor)
    {
        _progressionProcessor = progressionProcessor;
    }

    public ForwardRewardToProgressionResult Handle(
        ForwardRewardToProgressionCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.Reward is null)
        {
            throw new InvalidOperationException(
                "Reward result is required.");
        }

        var progressionRequest = new ProgressionRequest(
            command.CharacterId,
            command.Reward.Experience);

        _progressionProcessor.Process(
            progressionRequest);

        return new ForwardRewardToProgressionResult(
            command.CharacterId,
            true);
    }
}